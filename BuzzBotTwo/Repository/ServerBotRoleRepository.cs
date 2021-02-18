using BuzzBotTwo.Domain;

namespace BuzzBotTwo.Repository
{
    public class ServerBotRoleRepository:Repository<ServerBotRole, ulong>,IServerBotRoleRepository {
        public ServerBotRoleRepository(BotContext db) : base(db)
        {
        }
    }
}