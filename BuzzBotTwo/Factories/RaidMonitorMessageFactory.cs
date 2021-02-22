using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuzzBotTwo.Discord.Services;
using BuzzBotTwo.External.SoftResIt.Models;
using BuzzBotTwo.Repository;
using BuzzBotTwo.Utility;
using Discord;

namespace BuzzBotTwo.Factories
{
    public interface IRaidMonitorMessageFactory
    {
        Task<Embed> CreateRaidMonitorEmbed(RaidModel raidModel);
    }

    public class RaidMonitorMessageFactory : IRaidMonitorMessageFactory
    {
        private readonly IItemRepository _itemRepository;
        private readonly IEmoteService _emoteService;

        public RaidMonitorMessageFactory(IItemRepository itemRepository, IEmoteService emoteService)
        {
            _itemRepository = itemRepository;
            _emoteService = emoteService;
        }

        public async Task<Embed> CreateRaidMonitorEmbed(RaidModel raidModel)
        {
            var embedBuilder = new EmbedBuilder()
                .WithTitle($"Soft Res Raid Event - {raidModel.RaidId}")
                .AddField("Soft Res Sheet", $"[SoftRes.It](https://softres.it/raid/{raidModel.RaidId})", true)
                .AddField("Filler", "Filler", true)
                .AddField("Total Reserves", raidModel.Reserved.Count, true);
            var reservedCount = raidModel.Reserved.Count;
            if (reservedCount == 0) return embedBuilder.Build();
            var emptySpacesOnFinalRow = (3 - reservedCount % 3);
            var reservedGroups = raidModel.Reserved.SplitList(3).ToList();
            for (var i = 0; i < reservedGroups.Count; i++)
            {
                foreach (var softResUserModel in reservedGroups[i])
                {
                    var itemSb = new StringBuilder();
                    foreach (var item in softResUserModel.Items)
                    {
                        var itemData = await _itemRepository.FindAsync(item);
                        var itemString = itemData?.Name ?? $"Item#{item}";
                        itemSb.AppendLine(itemString);
                    }

                    embedBuilder.AddField($"{softResUserModel.Name}", itemSb.ToString(), true);
                }

                if (i != reservedGroups.Count) continue;
                for (var j = 0; j < emptySpacesOnFinalRow; j++)
                {
                    embedBuilder.AddField(EmbedConstants.EmptySpace, EmbedConstants.EmptySpace);
                }
            }

            return embedBuilder.Build();
        }
    }

    public static class EmbedConstants
    {
        public const string EmptySpace = "\u200b";

        public const string CasterEmoteName = @"caster";
        public const string MeleeEmoteName = @"melee";
        public const string RangedEmoteName = @"ranged";
        public const string TankEmoteName = @"tank";
        public const string HealerEmoteName = @"healer";

        public const string WarriorEmoteName = @"warrior";
        public const string PaladinEmoteName = @"paladin";
        public const string ShamanEmoteName = @"shaman";
        public const string HunterEmoteName = @"hunter";
        public const string RogueEmoteName = @"rogue";
        public const string DruidEmoteName = @"druid";
        public const string MageEmoteName = @"mage";
        public const string PriestEmoteName = @"priest";
        public const string WarlockEmoteName = @"warlock";
    }
}