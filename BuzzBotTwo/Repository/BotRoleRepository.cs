using BuzzBotTwo.Domain;

namespace BuzzBotTwo.Repository
{
    public class BotRoleRepository : Repository<BotRole, int>, IBotRoleRepository
    {
        public BotRoleRepository(BotContext db) : base(db)
        {
        }
    }
}