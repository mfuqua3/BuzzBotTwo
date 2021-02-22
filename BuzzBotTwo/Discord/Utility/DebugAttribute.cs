using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Discord.Commands;

namespace BuzzBotTwo.Discord.Utility
{
    public class DebugAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command,
            IServiceProvider services)
        {
            return Task.FromResult(Debugger.IsAttached
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError("Test commands are only valid in debug mode"));
        }
    }
}