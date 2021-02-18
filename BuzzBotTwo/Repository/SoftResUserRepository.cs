using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public class SoftResUserRepository:Repository<SoftResUser, ulong>,ISoftResUserRepository {
        public SoftResUserRepository(BotContext db) : base(db)
        {
        }
    }
}