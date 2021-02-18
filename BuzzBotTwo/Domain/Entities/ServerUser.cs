using System;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Domain
{
    public class ServerUser
    {
        public Guid ServerUserId { get; set; }
        public ulong ServerId { get; set; }
        public Server Server { get; set; }
        public ulong UserId { get; set; }
        public User User { get; set; }
        public int RoleId { get; set; }
        public BotRole Role { get; set; }
    }
}