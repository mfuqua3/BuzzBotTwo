using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BuzzBotTwo.Domain.Entities
{
    public class PaginatedMessage : IEntity<ulong>
    {
        public ulong Id { get; set; }
        public int CurrentPage { get; set; }
        public bool HasHiddenContent { get; set; }
        public bool IsRevealed { get; set; }
        public ulong MessageChannelId { get; set; }
        public MessageChannel MessageChannel { get; set; }
        public List<Page> Pages { get; set; }
        [NotMapped] 
        public List<Page> StandardPages => Pages?.Where(p => !p.IsHidden).ToList() ?? new List<Page>();
        [NotMapped]
        public List<Page> HiddenPages => Pages?.Where(p => p.IsHidden).ToList() ?? new List<Page>();
    }
}