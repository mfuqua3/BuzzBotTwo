using System;
using System.Collections.Generic;

namespace BuzzBotTwo.Domain.Entities
{
    public class SoftResUser
    {
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public int Spec { get; set; }
        public List<ReservedItem> ReservedItems { get; set; }
        public string Note { get; set; }
        public string EventId { get; set; }
        public SoftResEvent Event { get; set; }
        public Guid? ParticipantId { get; set; }
        public RaidParticipant Participant { get; set; }
    }

    public class ReservedItem
    {
        public ulong Id { get; set; }
        public ulong UserId { get; set; }
        public SoftResUser User { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
    }
}