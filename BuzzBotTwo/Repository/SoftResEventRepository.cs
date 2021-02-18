using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public class SoftResEventRepository : Repository<SoftResEvent, string>, ISoftResEventRepository
    {
        public SoftResEventRepository(BotContext db) : base(db)
        {
        }
    }
}