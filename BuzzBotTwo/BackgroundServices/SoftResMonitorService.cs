using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BuzzBotTwo.Configuration;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.External.SoftResIt;
using BuzzBotTwo.External.SoftResIt.Models;
using BuzzBotTwo.Factories;
using BuzzBotTwo.Repository;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BuzzBotTwo.BackgroundServices
{
    public class SoftResMonitorService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<SoftResRaidMonitor> _logger;
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly BackgroundProcessesConfiguration _config;

        public SoftResMonitorService(IServiceScopeFactory serviceScopeFactory, ILogger<SoftResRaidMonitor> logger,
            DiscordSocketClient discordSocketClient, IOptions<BackgroundProcessesConfiguration> config)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _discordSocketClient = discordSocketClient;
            _config = config.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var raidMonitorRepo = scope.ServiceProvider.GetRequiredService<ISoftResRaidMonitorRepository>();
                var softResClient = scope.ServiceProvider.GetRequiredService<ISoftResClient>();
                var messageFactory = scope.ServiceProvider.GetRequiredService<IRaidMonitorMessageFactory>();
                var raidMonitors = await raidMonitorRepo.GetAsync();
                var now = DateTime.UtcNow;
                foreach (var raidMonitor in raidMonitors)
                {
                    if (raidMonitor.RemoveAt < now)
                    {
                        await raidMonitorRepo.DeleteAsync(raidMonitor);
                        continue;
                    }
                    RaidModel softResRaid;
                    try
                    {
                        softResRaid = await softResClient.Query(raidMonitor.SoftResEventId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            $"An error occurred while querying raid ID \"{raidMonitor.SoftResEventId}\". Removing monitor.\n{ex}");
                        await raidMonitorRepo.DeleteAsync(raidMonitor);
                        continue;
                    }

                    var lastUpdate = softResRaid.Reserved.Max(c => c.Updated);
                    if (lastUpdate <= raidMonitor.LastUpdated)
                    {
                        continue;
                    }

                    var embed = await messageFactory.CreateRaidMonitorEmbed(softResRaid);
                    var channel = _discordSocketClient.GetChannel(raidMonitor.RaidChannelId) as IMessageChannel;
                    if (channel == null)
                    {
                        await raidMonitorRepo.DeleteAsync(raidMonitor);
                        continue;
                    }
                    var message = await channel.GetMessageAsync(raidMonitor.Id) as IUserMessage;
                    if (message == null)
                    {
                        await raidMonitorRepo.DeleteAsync(raidMonitor);
                        continue;
                    }
                    message.ModifyAsync(prop => prop.Embed = embed);
                    raidMonitor.LastUpdated = DateTime.UtcNow;
                }

                await raidMonitorRepo.SaveAllChangesAsync();
                await Task.Delay(TimeSpan.FromSeconds(_config.RaidMonitorRefreshRateSeconds), stoppingToken);
            }
        }
    }
}