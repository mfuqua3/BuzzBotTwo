using System.Linq;
using System.Threading.Tasks;
using BuzzBotTwo.Configuration;
using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.External.SoftResIt.Models;
using BuzzBotTwo.Repository;
using Discord;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BuzzBotTwo.Discord.Services
{
    public interface IUserDataService
    {
        Task EnsureCreated(IUser user, IGuild guild);
    }

    public class UserDataService : IUserDataService
    {
        private readonly IServerRepository _serverRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServerUserRepository _serverUserRepository;
        private readonly IBotRoleRepository _botRoleRepository;
        private readonly DiscordConfiguration _config;

        public UserDataService(IServerRepository serverRepository, IUserRepository userRepository,
            IServerUserRepository serverUserRepository, IBotRoleRepository botRoleRepository,
            IOptions<DiscordConfiguration> config)
        {
            _serverRepository = serverRepository;
            _userRepository = userRepository;
            _serverUserRepository = serverUserRepository;
            _botRoleRepository = botRoleRepository;
            _config = config.Value;
        }

        public async Task EnsureCreated(IUser user, IGuild guild)
        {
            var (serverCreated, userCreated) = (false, false);
            var server = await _serverRepository.FindAsync(guild.Id, (qry, ctx) => qry.Include(s => s.ServerBotRoles));
            if (server == null)
            {
                var botRoles = await _botRoleRepository.GetAsync();
                server = new Server
                {
                    Id = guild.Id,
                    ServerBotRoles = botRoles.Select(role => new ServerBotRole
                    {
                        Role = role
                    }).ToList()
                };
                await _serverRepository.PostAsync(server);
                serverCreated = true;
            }

            var userEntity = await _userRepository.FindAsync(user.Id);
            if (userEntity == null)
            {
                userEntity = new User
                {
                    Id = user.Id,
                };
                await _userRepository.PostAsync(userEntity);
                userCreated = true;
            }

            if (serverCreated || userCreated)
            {
                var botRole = user.Id != _config.OwnerId
                    ? server.ServerBotRoles.FirstOrDefault(br => br.RoleId == (int) BotRoleLevel.User)
                    : server.ServerBotRoles.FirstOrDefault(br => br.RoleId == (int) BotRoleLevel.Owner);
                var serverUser = new ServerUser
                {
                    Role = botRole,
                    Server = server,
                    User = userEntity
                };
                await _serverUserRepository.PostAsync(serverUser);
                await _serverUserRepository.SaveAllChangesAsync();
            }
        }
    }
}