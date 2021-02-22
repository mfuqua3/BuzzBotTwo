using System;
using System.Threading.Tasks;
using AutoMapper;
using BuzzBotTwo.Discord.Services;
using BuzzBotTwo.Discord.Utility;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.External.SoftResIt;
using BuzzBotTwo.External.SoftResIt.Models;
using BuzzBotTwo.Factories;
using BuzzBotTwo.Repository;
using Discord.Commands;
using Newtonsoft.Json;

namespace BuzzBotTwo.Discord.Modules
{
    [Group("test")]
    [Debug]
    public class TestModule : ModuleBase
    {
        private readonly IPageService _pageService;
        private readonly ISoftResClient _softClient;
        private readonly IRaidMonitorMessageFactory _raidMonitorMessageFactory;
        private readonly IMapper _mapper;
        private readonly ISoftResRaidMonitorRepository _raidMonitorRepository;
        private readonly IChannelService _channelService;
        private readonly ISoftResEventRepository _softResEventRepository;

        public TestModule(IPageService pageService, ISoftResClient softClient,
            IRaidMonitorMessageFactory raidMonitorMessageFactory, IMapper mapper,
            ISoftResRaidMonitorRepository raidMonitorRepository,
            IChannelService channelService, ISoftResEventRepository softResEventRepository)
        {
            _pageService = pageService;
            _softClient = softClient;
            _raidMonitorMessageFactory = raidMonitorMessageFactory;
            _mapper = mapper;
            _raidMonitorRepository = raidMonitorRepository;
            _channelService = channelService;
            _softResEventRepository = softResEventRepository;
        }

        [Admin]
        [Command("admin")]
        public async Task AdminTest()
        {
            await ReplyAsync("Task executed");
        }

        [Command("monitor")]
        public async Task TestRaidMonitor(string raidKey)
        {
            var raid = await _softClient.Query(raidKey);
            var embed = await _raidMonitorMessageFactory.CreateRaidMonitorEmbed(raid);
            var message = await ReplyAsync("", false, embed);
            var softResRaidEntity = await _softResEventRepository.FindAsync(raidKey) ?? new SoftResEvent();
            _mapper.Map<RaidModel, SoftResEvent>(raid, softResRaidEntity);
            var raidMonitor = new SoftResRaidMonitor()
            {
                CreatedAt = DateTime.UtcNow,
                RemoveAt = DateTime.UtcNow + TimeSpan.FromMinutes(15),
                Id = message.Id,
                RaidChannel = await _channelService.AddOrGetChannel(Context),
                SoftResEvent = softResRaidEntity
            };
            await _raidMonitorRepository.PostAsync(raidMonitor);
            await _raidMonitorRepository.SaveAllChangesAsync();
        }

        [Command("srEmbed")]
        public async Task TestSoftResEmbed(string raidKey)
        {
            var raid = await _softClient.Query(raidKey);
            var embed = await _raidMonitorMessageFactory.CreateRaidMonitorEmbed(raid);
            await ReplyAsync("", false, embed);
        }

        [Command("softRes")]
        public async Task QuerySoftResRaid(string raidKey)
        {
            try
            {
                var raid = await _softClient.Query(raidKey);
                await ReplyAsync($"```json\n{JsonConvert.SerializeObject(raid, Formatting.Indented)}\n```");
            }
            catch (Exception ex)
            {
                await ReplyAsync(ex.Message);
            }
        }

        [Command("pageTest")]
        public async Task TestPagination()
        {
            var pageBuilder = new PageFormatBuilder()
                .AddColumn("Col1")
                .AddColumn("Col2")
                .AddColumn("Col3")
                .AddHiddenColumn("HiddenCol1")
                .AlternateRowColors();
            for (var i = 0; i < 50; i++)
            {
                pageBuilder.AddRow(new[] {$"Col1:Row{i}", $"Col2:Row{i}", $"Col3:Row{i}", $"HiddenCol1:Row{i}",});
            }

            await _pageService.SendPages(Context.Channel, pageBuilder.Build());
        }
    }
}