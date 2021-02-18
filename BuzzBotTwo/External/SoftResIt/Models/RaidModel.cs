using System;
using System.Collections.Generic;

namespace BuzzBotTwo.External.SoftResIt.Models
{
    public class RaidModel
    {
        public string RaidId { get; set; }
        public string Instance { get; set; }
        public bool? Discord { get; set; }
        public string DiscordId { get; set; }
        public string DiscordInvite { get; set; }
        public string Token { get; set; }
        public List<SoftResUserModel> Reserved { get; set; }
        public string Faction { get; set; }
        public int Amount { get; set; }
        public bool? Lock { get; set; }
        public string Note { get; set; }
        public string RaidDate { get; set; }
        public bool? LockRaidDate { get; set; }
        public bool? HideReserves { get; set; }
        public int? ItemLimit { get; set; }
        public int? PlusModifier { get; set; }
        public bool? RestrictByClass { get; set; }
        public bool? CharacterNotes { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? Updated { get; set; }
    }
}