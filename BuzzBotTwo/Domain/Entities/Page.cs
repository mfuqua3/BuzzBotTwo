using System;
using Discord;

namespace BuzzBotTwo.Domain.Entities
{
    public class Page : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public int PageNumber { get; set; }
        public ulong MessageId { get; set; }
        public bool IsHidden { get; set; }
        public PaginatedMessage Message { get; set; }
        public string Content { get; set; }
    }
}