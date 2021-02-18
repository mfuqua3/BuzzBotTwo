using System;

namespace BuzzBotTwo.Domain.Entities
{
    public class RaidParticipant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ServerUserId { get; set; }
        public ServerUser ServerUser { get; set; }
    }
}