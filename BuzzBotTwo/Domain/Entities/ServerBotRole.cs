using System.Collections.Generic;

namespace BuzzBotTwo.Domain
{
    public class ServerBotRole
    {
        public ulong Id { get; set; }
        public ulong ServerId { get; set; }
        public Server Server { get; set; }
        public List<ServerUser> Users { get; set; }
        public int RoleId { get; set; }
        public BotRole Role { get; set; }
    }
}