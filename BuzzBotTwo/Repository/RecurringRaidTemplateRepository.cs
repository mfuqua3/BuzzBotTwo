using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public class RecurringRaidTemplateRepository:Repository<RecurringRaidTemplate, int>, IRecurringRaidTemplateRepository {
        public RecurringRaidTemplateRepository(BotContext db) : base(db)
        {
        }
    }
}