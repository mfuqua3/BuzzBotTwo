using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public class RaidRepository : Repository<Raid, int>, IRaidRepository
    {
        public RaidRepository(BotContext db) : base(db)
        {
        }
    }
}