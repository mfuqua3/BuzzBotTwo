using System;
using System.Linq;
using System.Threading.Tasks;
using BuzzBotTwo.Discord.Services;
using BuzzBotTwo.Domain;
using BuzzBotTwo.Repository;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuzzBotTwo.Discord.Utility
{
    public class AdminAttribute : PreconditionAttribute
    {
        public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context,
            CommandInfo command, IServiceProvider services)
        {
            var serverRepository = services.GetRequiredService<IServerRepository>();
            var userDataService = services.GetRequiredService<IUserDataService>();
            await userDataService.EnsureCreated(context.User, context.Guild);
            var server = await serverRepository.FindAsync(context.Guild.Id,
                (query, _) => query.Include(s => s.ServerUsers).ThenInclude(usr => usr.Role));
            var user = server?.ServerUsers.FirstOrDefault(usr => usr.UserId == context.User.Id);
            if (user == null) return PreconditionResult.FromError("User permission data not found.");
            return user.Role.RoleId <= (int) BotRoleLevel.Admin
                ? PreconditionResult.FromSuccess()
                : PreconditionResult.FromError("User not authorized for command");
        }
    }
}