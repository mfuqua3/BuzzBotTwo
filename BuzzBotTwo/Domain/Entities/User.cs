using System.Collections.Generic;

namespace BuzzBotTwo.Domain.Entities
{
    public class User:IEntity<ulong>
    {
        public ulong Id { get; set; }
        public List<ServerUser> ServerUsers { get; set; } 
    }
}