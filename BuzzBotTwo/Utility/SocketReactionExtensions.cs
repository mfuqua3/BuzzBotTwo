using System.Linq;
using Discord.WebSocket;

namespace BuzzBotTwo.Utility
{
    public static class SocketReactionExtensions
    {
        public static bool ValidateReaction(this SocketReaction reaction, params string[] validReactionStrings)
        {
            return !reaction.User.Value.IsBot && validReactionStrings.Any(vrs => vrs.Equals(reaction.Emote.Name));
        }
    }
}