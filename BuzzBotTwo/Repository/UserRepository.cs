using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public class UserRepository : Repository<User, ulong>, IUserRepository
    {
        public UserRepository(BotContext db) : base(db)
        {
        }
    }
}