using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using BuzzBotTwo.Discord.Services;
using BuzzBotTwo.Discord.Utility;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.External.SoftResIt;
using BuzzBotTwo.External.SoftResIt.Models;
using BuzzBotTwo.Repository;
using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;

namespace BuzzBotTwo.Discord.Modules
{
    [Admin]
    [Group("raid")]
    public class RaidModule : BotModule
    {
        private readonly IRecurringRaidTemplateRepository _recurringRaidTemplateRepository;
        private readonly ISoftResRaidTemplateRepository _softResRaidTemplateRepository;
        private readonly ITemplateConfigurationService _templateConfigurationService;
        private readonly IPageService _pageService;
        private readonly ISoftResClient _softResClient;
        private readonly IMapper _mapper;
        private readonly ISoftResEventRepository _softResEventRepository;
        private readonly IChannelService _channelService;
        private IRaidRepository _raidRepository;

        public RaidModule(IUserDataService userDataService,
            IRecurringRaidTemplateRepository recurringRaidTemplateRepository,
            ISoftResRaidTemplateRepository softResRaidTemplateRepository,
            ITemplateConfigurationService templateConfigurationService,
            IPageService pageService, ISoftResClient softResClient, IMapper mapper,
            ISoftResEventRepository softResEventRepository, IChannelService channelService,
            IRaidRepository raidRepository) : base(userDataService)
        {
            _recurringRaidTemplateRepository = recurringRaidTemplateRepository;
            _softResRaidTemplateRepository = softResRaidTemplateRepository;
            _templateConfigurationService = templateConfigurationService;
            _pageService = pageService;
            _softResClient = softResClient;
            _mapper = mapper;
            _softResEventRepository = softResEventRepository;
            _channelService = channelService;
            _raidRepository = raidRepository;
        }

        [Command("begin")]
        public async Task BeginRaid(string templateKey)
        {
            var softResTemplate = await _softResRaidTemplateRepository.FindAsync(templateKey);
            if (softResTemplate == null)
                await ReplyAsync("No template by that name has been configured.");
            var raid = await _softResClient.CreateRaid(opt =>
                opt.ForFaction(softResTemplate.Faction)
                    .ForInstance(softResTemplate.Instance)
                    .TotalReserves(softResTemplate.ReserveAmounts));
            var softResEntity = _mapper.Map<RaidModel, SoftResEvent>(raid);
            await _softResEventRepository.PostAsync(softResEntity);
            var embedBuilder = new EmbedBuilder().WithTitle($"Raid Event - {DateTime.Now.ToString("M")}")
                .AddField("Soft Res Sheet", $"[SoftRes.It](https://softres.it/raid/{raid.RaidId})", true)
                .AddField("Test", "Test", true)
                .AddField("Test", "Test", true)
                .WithTimestamp(DateTimeOffset.Now);
            var message = await ReplyAsync("", false, embedBuilder.Build());
            var userDM = await Context.User.GetOrCreateDMChannelAsync();
            await userDM.SendMessageAsync($"Soft Reserve Raid created successfully. Token = {raid.Token}");
            var raidEntity = new Raid()
            {
                SoftResEventId = raid.RaidId,
                StartTime = DateTime.UtcNow,
                Channel = await _channelService.AddOrGetChannel(Context),
                MessageId = message.Id
            };
            await _raidRepository.PostAsync(raidEntity);
            await _raidRepository.SaveAllChangesAsync();
        }

        [Command("update")]
        public async Task UpdateConfig(int key, string value)
        {
            if (key == typeof(RecurringRaidTemplate)
                .GetProperty(nameof(RecurringRaidTemplate.SoftResTemplateId))
                .GetCustomAttribute<ConfigurationKeyAttribute>().Key)
            {
                var templateExists = await _softResRaidTemplateRepository.Contains(value);
                if (!templateExists)
                {
                    await ReplyAsync("No such soft res template exists.");
                    return;
                }
            }

            var config = await AddOrGetRaidTemplate(false);
            _templateConfigurationService.UpdateTemplate(config, key, value);
            await _recurringRaidTemplateRepository.SaveAllChangesAsync();
            await ReplyAsync("Configuration updated successfully");
        }

        private async Task<RecurringRaidTemplate> AddOrGetRaidTemplate(bool saveChanges = true)
        {
            var config = (await _recurringRaidTemplateRepository.GetAsync((qry, _) =>
                    qry.Where(cfg => cfg.ServerId == Context.Guild.Id)))
                .FirstOrDefault();
            if (config == null)
            {
                config = new RecurringRaidTemplate
                {
                    SoftResTemplateId = string.Empty,
                    ServerId = Context.Guild.Id,
                    ResetDayOfWeek = (int) DayOfWeek.Monday
                };
                await _recurringRaidTemplateRepository.PostAsync(config);
                if (saveChanges)
                    await _recurringRaidTemplateRepository.SaveAllChangesAsync();
            }

            return config;
        }

        [Command("config")]
        public async Task PrintConfig()
        {
            var config = await AddOrGetRaidTemplate();

            var configPropertyData =
                _templateConfigurationService.GetPropertyData(config).ToList().OrderBy(pi => pi.Key);
            var pageBuilder = new PageFormatBuilder()
                .AddColumn("Property Key")
                .AddColumn("Property Name")
                .AddColumn("Current Value")
                .AlternateRowColors();
            foreach (var kvp in configPropertyData)
            {
                var valueString = kvp.Value.ToString();
                if (kvp.Key == typeof(RecurringRaidTemplate)
                    .GetProperty(nameof(RecurringRaidTemplate.ResetDayOfWeek))
                    .GetCustomAttribute<ConfigurationKeyAttribute>().Key)
                {
                    valueString = $"{kvp.Value} ({((DayOfWeek) kvp.Value)})";
                }

                pageBuilder.AddRow(new[] {kvp.Key.ToString(), kvp.Name, valueString});
            }

            await _pageService.SendPages(Context.Channel, pageBuilder.Build());
        }
    }
}