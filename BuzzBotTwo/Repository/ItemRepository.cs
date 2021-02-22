using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BuzzBotTwo.Repository
{
    public class ItemRepository : Repository<Item, int>, IItemRepository
    {
        public ItemRepository(BotContext db) : base(db)
        {
        }

        public async Task<List<Item>> QueryItem(string queryString)
        {
            return await Db.Items.AsQueryable().Where(itm => EF.Functions.Like(itm.Name, $"%{queryString}%"))
                .ToListAsync();
        }
    }
}