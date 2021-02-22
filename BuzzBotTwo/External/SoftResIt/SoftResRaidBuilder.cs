using System;
using BuzzBotTwo.External.SoftResIt.Models;
using BuzzBotTwo.Utility;

namespace BuzzBotTwo.External.SoftResIt
{
    public class SoftResRaidBuilder : ISoftResRaidBuilder
    {
        private RaidModel _raidModel;

        public SoftResRaidBuilder()
        {
            _raidModel =  new RaidModel
            {
                Amount = 2,
                Faction = "Horde",
                Discord = false,
                Instance = "naxxramas",
                ItemLimit = 0,
                HideReserves = false,
                CharacterNotes = true,
                PlusModifier = 1,
                RestrictByClass = true
            };
        }
        public ISoftResRaidBuilder StartAt(DateTime startTime)
        {
            _raidModel.RaidDate = startTime.ToString("u");
            return this;
        }

        public ISoftResRaidBuilder LockStart()
        {
            _raidModel.LockRaidDate = true;
            return this;
        }

        public ISoftResRaidBuilder ForInstance(SoftResInstance instance)
        {
            _raidModel.Instance = instance.GetAttributeOfType<SoftResKeyAttribute>().Key;
            return this;
        }

        public ISoftResRaidBuilder ForInstance(string instance)
        {
            _raidModel.Instance = instance;
            return this;
        }

        public ISoftResRaidBuilder TotalReserves(int amount)
        {
            _raidModel.Amount = amount;
            return this;
        }

        public ISoftResRaidBuilder ForFaction(SoftResFaction faction)
        {
            _raidModel.Faction = faction.GetAttributeOfType<SoftResKeyAttribute>().Key;
            return this;
        }

        public ISoftResRaidBuilder ForFaction(string faction)
        {
            _raidModel.Faction = faction;
            return this;
        }

        public ISoftResRaidBuilder HideReserves()
        {
            _raidModel.HideReserves = true;
            return this;
        }

        public RaidModel Build()
        {
            return _raidModel;
        }
    }
}