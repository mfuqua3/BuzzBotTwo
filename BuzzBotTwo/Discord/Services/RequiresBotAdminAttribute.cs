﻿using System;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace BuzzBotTwo.Discord.Services
{
    public class RequiresBotAdminAttribute:PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            return services.GetService<IAdministrationService>().IsUserAdmin(context.User)
                ? Task.FromResult(PreconditionResult.FromSuccess())
                : Task.FromResult(PreconditionResult.FromError($"User not authorized for \"{command.Name}\" command"));
        }
    }
}