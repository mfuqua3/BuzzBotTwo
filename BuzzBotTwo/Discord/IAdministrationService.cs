using Discord;

namespace BuzzBotTwo.Discord
{
    public interface IAdministrationService
    {
        bool IsUserAdmin(IUser user);
        void Authorize(IUser user);
        bool IsUserAdmin(ulong requestingUser);
    }
}