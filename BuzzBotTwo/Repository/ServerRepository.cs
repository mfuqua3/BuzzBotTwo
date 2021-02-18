using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public class ServerRepository : Repository<Server, ulong>, IServerRepository
    {
        public ServerRepository(BotContext db) : base(db)
        {
        }
    }
}