using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

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
                .AddSingleton<IAdministrationService, AdministrationService>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>();
            return services;
        }
    }
}