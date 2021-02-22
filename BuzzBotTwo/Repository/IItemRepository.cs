using System.Collections.Generic;
using System.Threading.Tasks;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public interface IItemRepository : IRepository<Item, int>
    {
        Task<List<Item>> QueryItem(string queryString);
    }
}