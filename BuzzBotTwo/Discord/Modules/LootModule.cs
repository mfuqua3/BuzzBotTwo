using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuzzBotTwo.Discord.Services;
using BuzzBotTwo.Discord.Utility;
using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.Repository;
using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;

namespace BuzzBotTwo.Discord.Modules
{
    [Admin]
    public class LootModule : BotModule
    {
        private readonly IServerRepository _serverRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IQueryService _queryService;

        public LootModule(IUserDataService userDataService, IServerRepository serverRepository,
            IItemRepository itemRepository, IQueryService queryService) : base(userDataService)
        {
            _serverRepository = serverRepository;
            _itemRepository = itemRepository;
            _queryService = queryService;
        }

        [Command("assign", RunMode = RunMode.Async)]
        public async Task AssignItem(IUser user, [Remainder] string itemName)
        {
            UserDataService.EnsureCreated(user, Context.Guild);
            var server =
                await _serverRepository.FindAsync(Context.Guild.Id, (qry, context) => qry.Include(s => s.ServerUsers));
            var items = (await _itemRepository.QueryItem(itemName)).OrderByDescending(item=>item.ItemLevel).ToList();
            if (!items.Any())
            {
                await ReplyAsync($"\"{itemName}\" returned no results.");
                return;
            }

            var item = items.First();
            if(items.Count>1)
            {
                var selection = await _queryService.SendOptionSelectQuery(
                    $"\"{itemName}\" yielded multiple results. Please select below",
                    items,
                    item => item.Name,
                    Context.Channel, CancellationToken.None);
                if (selection == -1)
                {
                    await ReplyAsync("Operation cancelled");
                    return;
                }

                item = items[selection];
            }

            await ReplyAsync("", false, CreateItemEmbed(item));
        }

        private Embed CreateItemEmbed(Item item)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle($"Assigning {item.Name}");
            embed.WithImageUrl($"http://www.korkd.com/wow_img/{item.Id}.png");
            return embed.Build();
        }
    }
}