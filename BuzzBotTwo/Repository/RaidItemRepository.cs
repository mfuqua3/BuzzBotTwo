using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public class RaidItemRepository : Repository<RaidItem, uint>, IRaidItemRepository
    {
        public RaidItemRepository(BotContext db) : base(db)
        {
        }
    }
}