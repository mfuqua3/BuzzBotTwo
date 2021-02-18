using System.Threading.Tasks;
using BuzzBotTwo.Discord.Utility;
using Discord;

namespace BuzzBotTwo.Discord.Services
{
    public interface IPageService
    {
        Task SendPages(IMessageChannel channel, PageFormat pageFormat);
        Task SendPages(IMessageChannel channel, string header, params string[] contentLines);
    }
}