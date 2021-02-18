using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BuzzBotTwo.Discord.Services;
using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Domain.Entities
{
    public class Server : IEntity<ulong>
    {
        [Key]
        public ulong Id { get; set; }
        public List<ServerBotRole> ServerBotRoles { get; set; }
        public List<ServerUser> ServerUsers { get; set; }
    }

    public class RecurringRaidTemplate : IEntity<int>
    {
        public int Id { get; set; }
        public ulong ServerId { get; set; }
        public Server Server { get; set; }
        [ConfigurationKey(2)]
        [DisplayName("Use Soft Res Template")]
        public string SoftResTemplateId { get; set; }
        public SoftResRaidTemplate SoftResTemplate { get; set; }
        [ConfigurationKey(1)]
        [DisplayName("Post New Raid On")]
        public int ResetDayOfWeek { get; set; }
    }
    public class SoftResRaidTemplate : IEntity<string>
    {
        public string Id { get; set; }
        public ulong ServerId { get; set; }
        public Server Server { get; set; }
        public string Instance { get; set; }
        public string Faction { get; set; }
        [ConfigurationKey(1)]
        [DisplayName("Total Reserves Allowed")]
        public int ReserveAmounts { get; set; }
    }
}