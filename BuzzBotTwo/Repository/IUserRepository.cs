using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public interface IUserRepository:IRepository<User, ulong> { }
}