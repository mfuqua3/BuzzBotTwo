using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuzzBotTwo.Domain
{
    public class Server
    {
        [Key]
        public ulong Id { get; set; }
        public ulong RaidChannel { get; set; }
        public List<ServerBotRole> ServerBotRoles { get; set; }
        public List<ServerUser> ServerUsers { get; set; }
    }
}