using System.Collections.Generic;

namespace BuzzBotTwo.Domain.Entities
{
    public class User
    {
        public ulong Id { get; set; }
        public List<ServerUser> ServerUsers { get; set; } 
    }
}