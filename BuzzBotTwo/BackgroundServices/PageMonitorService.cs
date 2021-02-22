using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuzzBotTwo.Repository;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuzzBotTwo.BackgroundServices
{
    public class PageMonitorService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly DiscordSocketClient _socketClient;
        private readonly ConcurrentQueue<ReactionQueueItem> _reactionQueue = new ConcurrentQueue<ReactionQueueItem>();
        private const string ArrowBackward = @"⬅️";
        private const string ArrowForward = @"➡️";
        private IEmote _revealEmote = new Emoji("🔎");

        private enum PageAction
        {
            None,
            Forward,
            Backward,
            Reveal
        }

        public PageMonitorService(IServiceScopeFactory scopeFactory, DiscordSocketClient socketClient)
        {
            _scopeFactory = scopeFactory;
            _socketClient = socketClient;
            socketClient.ReactionAdded += ReactionManipulated;
            socketClient.ReactionRemoved += ReactionManipulated;

        }

        private Task ReactionManipulated(Cacheable<IUserMessage, ulong> _, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.User.Value.IsBot)
            {
                return Task.CompletedTask;
            }
            var action = reaction.Emote switch
            {
                {Name: ArrowForward} => PageAction.Forward,
                {Name: ArrowBackward} => PageAction.Backward,
                { } emote when Equals(emote, _revealEmote) => PageAction.Reveal,
                _ => PageAction.None
            };

            if (action != PageAction.None)
            {
                _reactionQueue.Enqueue(new ReactionQueueItem(channel, reaction, action));
            }
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                while (_reactionQueue.TryDequeue(out var queueItem))
                {
                    using var scope = _scopeFactory.CreateScope();
                    var channelRepository = scope.ServiceProvider.GetRequiredService<IMessageChannelRepository>();
                    var messageChannel = await channelRepository.FindAsync(queueItem.MessageChannel.Id,
                        (query, context) =>
                            query.Include(channel => channel.PaginatedMessages).ThenInclude(msg => msg.Pages));
                    var message =
                        messageChannel?.PaginatedMessages.FirstOrDefault(msg => msg.Id == queueItem.Reaction.MessageId);
                    if (message == null)
                        continue;
                    var messageObj = (await queueItem.Reaction.Channel.GetMessageAsync(message.Id));
                    if (!(messageObj is IUserMessage userMessage)) continue;
                    switch (queueItem.Action)
                    {
                        case PageAction.Forward:
                            message.CurrentPage += 1;
                            break;
                        case PageAction.Backward:
                            message.CurrentPage -= 1;
                            break;
                        case PageAction.Reveal:
                            message.IsRevealed = !message.IsRevealed;
                            break;
                        default:
                            continue;
                    }
                    var pageCollection = message.HasHiddenContent && message.IsRevealed
                        ? message.HiddenPages
                        : message.StandardPages;
                    var totalPages = pageCollection.Count;

                    if (message.CurrentPage <= 0) message.CurrentPage = totalPages;
                    if (message.CurrentPage > totalPages) message.CurrentPage = 1;
                    var pageToSend = pageCollection.OrderBy(p => p.PageNumber).ToArray()[message.CurrentPage - 1];
                    await userMessage.ModifyAsync(opt => opt.Content = pageToSend.Content);
                    await channelRepository.SaveAllChangesAsync();
                }

                await Task.Delay(TimeSpan.FromMilliseconds(100), stoppingToken);
            }
        }

        private class ReactionQueueItem
        {
            public ReactionQueueItem(ISocketMessageChannel messageChannel, SocketReaction reaction, PageAction action)
            {
                MessageChannel = messageChannel;
                Reaction = reaction;
                Action = action;
            }

            public ISocketMessageChannel MessageChannel { get; }
            public SocketReaction Reaction { get; }
            public PageAction Action { get; }
        }

    }
}