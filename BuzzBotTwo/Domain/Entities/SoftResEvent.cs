using System;
using System.Collections.Generic;

namespace BuzzBotTwo.Domain.Entities
{
    public class SoftResEvent
    {
        public string Id { get; set; }
        public string Instance { get; set; }
        public string Token { get; set; }
        public string Faction { get; set; }
        public int Amount { get; set; }
        public string Note { get; set; }
        public DateTime? RaidDate { get; set; }
        public int? ItemLimit { get; set; }
        public List<SoftResUser> Users { get; set; }
    }
}