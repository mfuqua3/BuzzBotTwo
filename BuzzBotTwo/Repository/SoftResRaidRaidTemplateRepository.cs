using BuzzBotTwo.Domain;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Repository
{
    public class SoftResRaidRaidTemplateRepository : Repository<SoftResRaidTemplate, string>, ISoftResRaidTemplateRepository
    {
        public SoftResRaidRaidTemplateRepository(BotContext db) : base(db)
        {
        }
    }
}