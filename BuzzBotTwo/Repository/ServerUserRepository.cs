using System;
using BuzzBotTwo.Domain;

namespace BuzzBotTwo.Repository
{
    public class ServerUserRepository:Repository<ServerUser,Guid>, IServerUserRepository
    {
        public ServerUserRepository(BotContext db) : base(db)
        {
        }
    }
}