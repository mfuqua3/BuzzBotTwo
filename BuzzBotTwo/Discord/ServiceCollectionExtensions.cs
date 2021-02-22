using BuzzBotTwo.Discord.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BuzzBotTwo.Discord
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscordComponents(this IServiceCollection services)
        {
            var intents =
                GatewayIntents.GuildMembers |
                GatewayIntents.DirectMessages |
                GatewayIntents.GuildMessageReactions |
                GatewayIntents.DirectMessageReactions |
                GatewayIntents.GuildMessages |
                GatewayIntents.GuildMessageReactions |
                GatewayIntents.Guilds |
                GatewayIntents.GuildPresences;
            var config = new DiscordSocketConfig()
            {
                GatewayIntents = intents
            };
            var client = new DiscordSocketClient(config);
            services.AddSingleton(client)
                .AddScoped<ScopedCommandContext>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddTransient<IQueryService, QueryService>()
                .AddScoped<IPageService, PageService>()
                .AddTransient<IEmoteService, EmoteService>();
            return services;
        }
    }
}