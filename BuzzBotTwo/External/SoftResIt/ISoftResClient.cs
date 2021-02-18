using System;
using System.Threading.Tasks;
using BuzzBotTwo.External.SoftResIt.Models;

namespace BuzzBotTwo.External.SoftResIt
{
    public interface ISoftResClient
    {
        Task<RaidModel> CreateRaid(SoftResRaidBuilderExpression builderExpression);
    }

    public delegate ISoftResRaidBuilder SoftResRaidBuilderExpression(ISoftResRaidBuilder builder);
}