using System;
using BuzzBotTwo.Domain;

namespace BuzzBotTwo.Repository
{
    public interface IServerUserRepository:IRepository<ServerUser, Guid> { }
}