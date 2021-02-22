using System;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Domain
{
    public class ServerUser:IEntity<Guid>
    {
        public Guid Id { get; set; }
        public ulong ServerId { get; set; }
        public Server Server { get; set; }
        public ulong UserId { get; set; }
        public User User { get; set; }
        public ulong RoleId { get; set; }
        public ServerBotRole Role { get; set; }
    }
}