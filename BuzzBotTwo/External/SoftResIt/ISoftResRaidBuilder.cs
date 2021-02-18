using System;
using BuzzBotTwo.External.SoftResIt.Models;

namespace BuzzBotTwo.External.SoftResIt
{
    public interface ISoftResRaidBuilder
    {
        ISoftResRaidBuilder StartAt(DateTime startTime);
        ISoftResRaidBuilder LockStart();
        ISoftResRaidBuilder ForInstance(SoftResInstance instance);
        ISoftResRaidBuilder TotalReserves(int amount);
        ISoftResRaidBuilder ForFaction(SoftResFaction faction);
        ISoftResRaidBuilder HideReserves();
        RaidModel Build();
    }
}