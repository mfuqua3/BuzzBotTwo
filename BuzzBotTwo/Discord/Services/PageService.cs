using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuzzBotTwo.Discord.Utility;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.Factories;
using BuzzBotTwo.Repository;
using Discord;
using Discord.WebSocket;

namespace BuzzBotTwo.Discord.Services
{
    public class PageService : IPageService
    {
        private readonly IMessageChannelRepository _messageChannelRepository;
        private const string ArrowBackward = @"⬅️";
        private const string ArrowForward = @"➡️";
        private static int _id = 0;
        private readonly ConcurrentDictionary<IEmote, HashSet<int>> _revealEmoteDictionary = new ConcurrentDictionary<IEmote, HashSet<int>>();
        private readonly IMessageChannelFactory _messageChannelFactory;
        private readonly IPaginatedMessageRepository _paginatedMessageRepository;

        public PageService(DiscordSocketClient discordClient, IMessageChannelRepository messageChannelRepository, IMessageChannelFactory messageChannelFactory, IPaginatedMessageRepository paginatedMessageRepository)
        {
            _messageChannelRepository = messageChannelRepository;
            _messageChannelFactory = messageChannelFactory;
            _paginatedMessageRepository = paginatedMessageRepository;
        }

        private List<Page> BuildPages(BasePageFormat pageFormat)
        {
            var numberOfPages = (int)Math.Ceiling((double)pageFormat.ContentLines.Count / pageFormat.LinesPerPage);
            var contentLineQueue = new Queue<string>(pageFormat.ContentLines);
            var pageNumber = 0;
            var returnList = new List<Page>();
            while (contentLineQueue.Any())
            {
                pageNumber++;
                var longestLineLength = 0;
                var pageLineIterator = 0;
                var contentSb = new StringBuilder();
                contentSb.AppendLine("```diff");
                contentSb.AppendLine($"{pageFormat.HeaderLine}{(!string.IsNullOrEmpty(pageFormat.HorizontalRule) ? Environment.NewLine + pageFormat.HorizontalRule : string.Empty)}");
                while (pageLineIterator < pageFormat.LinesPerPage)
                {
                    if (!contentLineQueue.Any()) break;
                    var line = contentLineQueue.Dequeue();
                    if (line.Length > longestLineLength)
                        longestLineLength = line.Length;
                    contentSb.AppendLine(line);
                    pageLineIterator++;
                }
                var pageNumSb = new StringBuilder();
                pageNumSb.Append($"Page {pageNumber}/{numberOfPages}");
                while (pageNumSb.Length < longestLineLength)
                {
                    pageNumSb.Insert(0, ' ');
                }

                contentSb.AppendLine();
                contentSb.AppendLine(pageNumSb.ToString());
                contentSb.Append("```");
                var page = new Page
                {
                    Content = contentSb.ToString(),
                    PageNumber = pageNumber
                };
                returnList.Add(page);
            }

            return returnList;
        }

        public async Task SendPages(IMessageChannel channel, PageFormat pageFormat)
        {
            var channelObj = await _messageChannelRepository.FindAsync(channel.Id);
            if (channelObj == null)
            {
                channelObj = (channel is IDMChannel dmChannel)
                    ? _messageChannelFactory.CreateNewDM(channel.Id, dmChannel.Recipient.Id)
                    : null;
                channelObj = (channel is IGuildChannel guildChannel)
                    ? _messageChannelFactory.CreateNewServerMessage(channel.Id, guildChannel.GuildId)
                    : channelObj;
                if (channelObj == null) return;
                await _messageChannelRepository.PostAsync(channelObj);
            }

            var paginatedMessage = new PaginatedMessage()
            {
                Pages = BuildPages(pageFormat),
                HasHiddenContent = pageFormat.HasHiddenColumns
            };
            if (paginatedMessage.Pages.Count == 0) return;
            if (paginatedMessage.HasHiddenContent)
            {
                var revealedPages = BuildPages(pageFormat.RevealedPageFormat);
                revealedPages.ForEach(p => p.IsHidden = true);
                paginatedMessage.Pages.AddRange(revealedPages);
            }
            var message = await channel.SendMessageAsync(paginatedMessage.Pages.First().Content);
            paginatedMessage.Id = message.Id;
            paginatedMessage.MessageChannelId = channelObj.Id;
            if (paginatedMessage.Pages.Count != 1)
            {
                if (paginatedMessage.StandardPages.Count > 1)
                {
                    await message.AddReactionAsync(new Emoji(ArrowBackward));
                    await message.AddReactionAsync(new Emoji(ArrowForward));
                }

                if (pageFormat.HasHiddenColumns)
                    await message.AddReactionAsync(pageFormat.RevealEmote);
            }
            await _paginatedMessageRepository.PostAsync(paginatedMessage);
            await _messageChannelRepository.SaveAllChangesAsync();
        }

        public async Task SendPages(IMessageChannel channel, string header, params string[] contentLines)
            => await SendPages(channel,
                new PageFormat { HeaderLine = header, ContentLines = contentLines.ToList(), HorizontalRule = null, LinesPerPage = 15 });


    }
}