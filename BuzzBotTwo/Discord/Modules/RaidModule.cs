using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuzzBotTwo.Discord.Services;
using BuzzBotTwo.Discord.Utility;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.Repository;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;

namespace BuzzBotTwo.Discord.Modules
{
    [Group("raid")]
    public class RaidModule : BotModule
    {
        private readonly IRecurringRaidTemplateRepository _recurringRaidTemplateRepository;
        private readonly ISoftResRaidTemplateRepository _softResRaidTemplateRepository;
        private readonly ITemplateConfigurationService _templateConfigurationService;
        private readonly IPageService _pageService;

        public RaidModule(IUserDataService userDataService,
            IRecurringRaidTemplateRepository recurringRaidTemplateRepository,
            ISoftResRaidTemplateRepository softResRaidTemplateRepository,
            ITemplateConfigurationService templateConfigurationService,
            IPageService pageService) : base(userDataService)
        {
            _recurringRaidTemplateRepository = recurringRaidTemplateRepository;
            _softResRaidTemplateRepository = softResRaidTemplateRepository;
            _templateConfigurationService = templateConfigurationService;
            _pageService = pageService;
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

        public async Task<RecurringRaidTemplate> AddOrGetRaidTemplate(bool saveChanges = true)
        {
            var config = (await _recurringRaidTemplateRepository.GetAsync(qry =>
                    qry.Where(cfg => cfg.ServerId == Context.Guild.Id)))
                .FirstOrDefault();
            if (config == null)
            {
                config = new RecurringRaidTemplate
                {
                    SoftResTemplateId = string.Empty,
                    ServerId = Context.Guild.Id,
                    ResetDayOfWeek = (int)DayOfWeek.Monday
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
                    valueString = $"{kvp.Value} ({((DayOfWeek)kvp.Value)})";
                }
                pageBuilder.AddRow(new[] { kvp.Key.ToString(), kvp.Name, valueString });
            }

            await _pageService.SendPages(Context.Channel, pageBuilder.Build());

        }
    }
}