using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public class RaidLockoutRepository : Repository<RaidLockout, int>, IRaidLockoutRepository
    {
        public RaidLockoutRepository(BotContext db) : base(db)
        {
        }
    }
}