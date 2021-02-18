using System;

namespace BuzzBotTwo.Domain.Entities
{
    public class RaidItem:IEntity<uint>
    {
        public uint Id { get; set; }
        public DateTime? DateAwarded { get; set; }
        public Guid ParticipantId { get; set; }
        public RaidParticipant Participant { get; set; }
        public int RaidId { get; set; }
        public Raid Raid { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}