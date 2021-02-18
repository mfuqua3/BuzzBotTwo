using System;
using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public class RaidParticipantRepository : Repository<RaidParticipant, Guid>, IRaidParticipantRepository
    {
        public RaidParticipantRepository(BotContext db) : base(db)
        {
        }
    }
}