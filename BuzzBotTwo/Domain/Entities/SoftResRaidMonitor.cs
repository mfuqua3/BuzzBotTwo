using System;

namespace BuzzBotTwo.Domain.Entities
{
    public class SoftResRaidMonitor:IEntity<ulong>
    {
        public ulong Id { get; set; }
        public ulong RaidChannelId { get; set; }
        public MessageChannel RaidChannel { get; set; }
        public string SoftResEventId { get; set; }
        public SoftResEvent SoftResEvent { get; set; }
        public DateTime? LastUpdated { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime RemoveAt { get; set; }
    }
}