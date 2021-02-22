using System;
using System.Collections.Generic;

namespace BuzzBotTwo.Domain.Entities
{
    public class Raid:IEntity<int>
    {
        public int Id { get; set; }
        public ulong MessageId { get; set; }
        public ulong ChannelId { get; set; }
        public bool Active { get; set; }
        public MessageChannel Channel { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ulong ServerId { get; set; }
        public Server Server { get; set; }
        public string SoftResEventId { get; set; }
        public SoftResEvent SoftResEvent { get; set; }
        public List<RaidParticipant> Participants { get; set; }
        public List<RaidItem> Items { get; set; }
    }
}