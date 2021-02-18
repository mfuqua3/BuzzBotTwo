using System.Collections.Generic;
using Discord;

namespace BuzzBotTwo.Discord.Utility
{
    public class PageFormat:BasePageFormat
    {
        public bool HasHiddenColumns { get; set; }
        public IEmote RevealEmote { get; set; }
        public BasePageFormat RevealedPageFormat { get; set; }
    }

    public class BasePageFormat
    {
        public string HeaderLine { get; set; }
        public string HorizontalRule { get; set; }
        public List<string> ContentLines { get; set; }
        public int LinesPerPage { get; set; }
    }
}