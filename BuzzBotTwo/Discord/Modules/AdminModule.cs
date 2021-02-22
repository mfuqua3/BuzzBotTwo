using System.Linq;
using System.Threading.Tasks;
using BuzzBotTwo.Discord.Services;
using BuzzBotTwo.Discord.Utility;
using BuzzBotTwo.Domain;
using BuzzBotTwo.Repository;
using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;

namespace BuzzBotTwo.Discord.Modules
{
    [Admin]
    public class AdminModule : BotModule
    {
        private readonly IServerRepository _serverRepository;

        public AdminModule(IUserDataService userDataService, IServerRepository serverRepository) : base(userDataService)
        {
            _serverRepository = serverRepository;
        }

        [Command("authorize")]
        public async Task AuthorizeUser(IUser user)
        {
            await UserDataService.EnsureCreated(user, Context.Guild);
            var server = await _serverRepository.FindAsync(Context.Guild.Id,
                (query, _) => query
                    .Include(s => s.ServerUsers)
                    .Include(s => s.ServerBotRoles));
            var adminRole = server.ServerBotRoles.FirstOrDefault(role => role.RoleId == (int) BotRoleLevel.Admin);
            var serverUser = server.ServerUsers.FirstOrDefault(usr => usr.UserId == user.Id);
            serverUser.Role = adminRole;
            await _serverRepository.SaveAllChangesAsync();
            ReplyAsync("User authorized");
        }
    }
}