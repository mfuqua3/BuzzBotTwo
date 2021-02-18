using System;
using System.Linq;
using System.Threading.Tasks;
using BuzzBotTwo.Discord.Services;
using BuzzBotTwo.Discord.Utility;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.External.SoftResIt;
using BuzzBotTwo.Factories;
using BuzzBotTwo.Repository;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;

namespace BuzzBotTwo.Discord.Modules
{
    [Group(GroupName)]
    public class SoftResModule : BotModule
    {
        private readonly ISoftResRaidTemplateFactory _templateFactory;
        private readonly IRecurringRaidTemplateRepository _recurringRaidTemplateRepository;
        private readonly ISoftResRaidTemplateRepository _softResRaidTemplateRepository;
        private readonly ITemplateConfigurationService _templateConfigurationService;
        private readonly IPageService _pageService;
        public const string GroupName = "sr";

        public SoftResModule(
            ISoftResRaidTemplateFactory templateFactory,
            IRecurringRaidTemplateRepository recurringRaidTemplateRepository,
            ISoftResRaidTemplateRepository softResRaidTemplateRepository,
            IUserDataService userDataService,
            ITemplateConfigurationService templateConfigurationService,
            IPageService pageService)
        : base(userDataService)
        {
            _templateFactory = templateFactory;
            _recurringRaidTemplateRepository = recurringRaidTemplateRepository;
            _softResRaidTemplateRepository = softResRaidTemplateRepository;
            _templateConfigurationService = templateConfigurationService;
            _pageService = pageService;
        }
        [Command("add")]
        public async Task CreateTemplate(string id, int amount)
        {
            var newTemplate = _templateFactory.CreateNew(id, SoftResInstance.Naxxramas, SoftResFaction.Horde, amount);
            newTemplate.ServerId = Context.Guild.Id;
            var recurringTemplate = new RecurringRaidTemplate
            {
                ResetDayOfWeek = (int)DayOfWeek.Monday,
                ServerId = Context.Guild.Id,
                SoftResTemplate = newTemplate
            };
            await _recurringRaidTemplateRepository.PostAsync(recurringTemplate);
            await _recurringRaidTemplateRepository.SaveAllChangesAsync();
        }
        [Command("templates")]
        public async Task PrintTemplates()
        {
            var templates =
                await _recurringRaidTemplateRepository.GetAsync(qry =>
                    qry.Include(tmp => tmp.SoftResTemplate).Where(tmp => tmp.ServerId == Context.Guild.Id));
            var pageBuilder = new PageFormatBuilder()
                .AddColumn("Template Name")
                .AddColumn("Instance")
                .AddColumn("Allowed Reserves")
                .AlternateRowColors();
            foreach (var template in templates)
            {
                pageBuilder.AddRow(new[]
                {
                    template.SoftResTemplate.Id,
                    template.SoftResTemplate.Instance,
                    template.SoftResTemplate.ReserveAmounts.ToString()
                });
            }

            await _pageService.SendPages(Context.Channel, pageBuilder.Build());
        }
        [Command("template")]
        public async Task PrintTemplateInfo(string templateId)
        {

            var template = await GetTemplate(templateId);
            if (template == null)
            {
                await ReplyAsync("No template by that name could be found");
                return;
            }

            var kvps = _templateConfigurationService.GetPropertyData(template);
            var pageBuilder = new PageFormatBuilder()
                .AddColumn("Property Key")
                .AddColumn("Property Name")
                .AddColumn("Current Value")
                .AlternateRowColors();
            pageBuilder.AddRow(new[] { "N/A", "Template Name", template.Id });
            foreach (var kvp in kvps)
            {
                pageBuilder.AddRow(new[] { kvp.Key.ToString(), kvp.Name, kvp.Value.ToString() });
            }

            await _pageService.SendPages(Context.Channel, pageBuilder.Build());
        }
        [Command("update")]
        public async Task UpdateTemplate(string templateId, int key, int value)
        {
            var template = await GetTemplate(templateId);
            if (template == null)
            {
                await ReplyAsync("No template by that name could be found");
                return;
            }
            _templateConfigurationService.UpdateTemplate(template, key, value);
            await _softResRaidTemplateRepository.SaveAllChangesAsync();
            await ReplyAsync("Template updated successfully.");
        }

        private async Task<SoftResRaidTemplate> GetTemplate(string templateId)
        {
            return
                (await _softResRaidTemplateRepository
                    .GetAsync(qry => qry
                        .Where(tmp => tmp.ServerId == Context.Guild.Id)))
                .FirstOrDefault(tmp => tmp.Id == templateId);
        }
    }
}