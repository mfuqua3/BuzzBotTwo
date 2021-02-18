using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BuzzBotTwo.Configuration;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BuzzBotTwo.Discord
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IOptions<DiscordConfiguration> _configuration;
        private IServiceProvider _provider;

        public CommandHandlingService(IServiceProvider provider, DiscordSocketClient discord, CommandService commands, IOptions<DiscordConfiguration> configuration)
        {
            _discord = discord;
            _commands = commands;
            _configuration = configuration;
            _commands.CommandExecuted += CommandExecuted;
            _provider = provider;

            _discord.MessageReceived += MessageReceived;
        }

        private Task CommandExecuted(Optional<CommandInfo> commandInfo, ICommandContext context, IResult result)
        {
            var scopedContext = context as ScopedCommandContext;
            scopedContext?.ServiceScope.Dispose();
            return Task.CompletedTask;
        }

        public async Task InitializeAsync(IServiceProvider provider)
        {
            _provider = provider;
            using var scope = _provider.CreateScope();
            await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), scope.ServiceProvider);
            // Add additional initialization code here...
        }

        private async Task MessageReceived(SocketMessage rawMessage)
        {
            // Ignore system messages and messages from bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;
            var argPos = 0;
            //if (!message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) return;
            if (!message.HasCharPrefix('!', ref argPos) ||
                message.Author.IsBot) return;


            var scope = _provider.CreateScope();
            var context = new ScopedCommandContext(scope, _discord, message);
            var result = await _commands.ExecuteAsync(context, argPos, scope.ServiceProvider, MultiMatchHandling.Best);

            if (result.Error.HasValue
                /*&& result.Error.Value != CommandError.UnknownCommand*/)
                await context.Channel.SendMessageAsync(result.ToString());
        }

    }
}