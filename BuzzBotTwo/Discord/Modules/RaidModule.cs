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
        private readonly ITemplateConfigurationService _templateConfigurationService;
        private readonly IPageService _pageService;

        public RaidModule(IUserDataService userDataService,
            IRecurringRaidTemplateRepository recurringRaidTemplateRepository,
            ITemplateConfigurationService templateConfigurationService,
            IPageService pageService) : base(userDataService)
        {
            _recurringRaidTemplateRepository = recurringRaidTemplateRepository;
            _templateConfigurationService = templateConfigurationService;
            _pageService = pageService;
        }
        [Command("config")]
        public async Task PrintConfig()
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
                await _recurringRaidTemplateRepository.SaveAllChangesAsync();
            }

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
                pageBuilder.AddRow(new[] { kvp.Key.ToString(), kvp.Name, valueString });
            }

            await _pageService.SendPages(Context.Channel, pageBuilder.Build());

        }
    }
}