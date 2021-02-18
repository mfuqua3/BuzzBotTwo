using System.Threading.Tasks;
using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.Repository;
using Discord;

namespace BuzzBotTwo.Discord.Services
{
    public interface IUserDataService
    {
        Task EnsureCreated(IUser user, IGuild guild);
    }

    public class UserDataService:IUserDataService
    {
        private readonly IServerRepository _serverRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServerUserRepository _serverUserRepository;

        public UserDataService(IServerRepository serverRepository, IUserRepository userRepository, IServerUserRepository serverUserRepository)
        {
            _serverRepository = serverRepository;
            _userRepository = userRepository;
            _serverUserRepository = serverUserRepository;
        }
        public async Task EnsureCreated(IUser user, IGuild guild)
        {
            var (serverCreated, userCreated) = (false, false);
            var server = await _serverRepository.FindAsync(guild.Id);
            if (server == null)
            {
                server = new Server
                {
                    Id = guild.Id
                };
                await _serverRepository.PostAsync(server);
                serverCreated = true;
            }

            var userEntity = await _userRepository.FindAsync(user.Id);
            if (userEntity == null)
            {
                userEntity = new User
                {
                    Id = user.Id
                };
                await _userRepository.PostAsync(userEntity);
                userCreated = true;
            }

            if (serverCreated || userCreated)
            {
                var serverUser = new ServerUser
                {
                    RoleId = (int)BotRoleLevel.User,
                    Server = server,
                    User = userEntity
                };
                await _serverUserRepository.PostAsync(serverUser);
                await _serverUserRepository.SaveAllChangesAsync();
            }
        }
    }
}