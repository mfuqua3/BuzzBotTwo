using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public class ItemRepository : Repository<Item, int>, IItemRepository
    {
        public ItemRepository(BotContext db) : base(db)
        {
        }
    }
}