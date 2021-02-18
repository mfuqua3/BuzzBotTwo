using System;
using System.Collections.Generic;

namespace BuzzBotTwo.Domain.Entities
{
    public class RaidLockout:IEntity<int>
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Raid> Raids { get; set; }
    }
}