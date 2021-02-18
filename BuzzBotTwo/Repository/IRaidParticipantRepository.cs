using System;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public interface IRaidParticipantRepository:IRepository<RaidParticipant, Guid> { }
}