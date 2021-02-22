using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public class SoftResRaidMonitorRepository : Repository<SoftResRaidMonitor, ulong>, ISoftResRaidMonitorRepository
    {
        public SoftResRaidMonitorRepository(BotContext db) : base(db)
        {
        }
    }
}