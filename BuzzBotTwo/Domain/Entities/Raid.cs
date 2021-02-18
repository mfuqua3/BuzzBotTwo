using System;
using System.Collections.Generic;

namespace BuzzBotTwo.Domain.Entities
{
    public class Raid
    {
        public int Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public List<RaidParticipant> Participants { get; set; }
        public List<RaidItem> Items { get; set; }
    }
}