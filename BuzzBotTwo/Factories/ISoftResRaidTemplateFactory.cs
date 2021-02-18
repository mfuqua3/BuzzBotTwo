using BuzzBotTwo.Domain.Entities;
using BuzzBotTwo.External.SoftResIt;
using BuzzBotTwo.Utility;

namespace BuzzBotTwo.Factories
{
    public interface ISoftResRaidTemplateFactory
    {
        SoftResRaidTemplate CreateNew(string name, SoftResInstance instance, SoftResFaction faction,
            int reserveAmounts);
    }

    public class SoftResRaidTemplateFactory : ISoftResRaidTemplateFactory
    {
        public SoftResRaidTemplate CreateNew(string name, SoftResInstance instance, SoftResFaction faction, int reserveAmounts)
        {
            return new SoftResRaidTemplate
            {
                Id = name,
                Instance = instance.GetAttributeOfType<SoftResKeyAttribute>().Key,
                Faction = faction.GetAttributeOfType<SoftResKeyAttribute>().Key,
                ReserveAmounts = reserveAmounts
            };
        }
    }
}