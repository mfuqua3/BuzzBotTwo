using BuzzBotTwo.Discord.Services;
using Discord.Commands;

namespace BuzzBotTwo.Discord.Modules
{
    public class BotModule : ModuleBase
    {
        protected readonly IUserDataService UserDataService;

        public BotModule(IUserDataService userDataService)
        {
            UserDataService = userDataService;
        }
        protected override void BeforeExecute(CommandInfo command)
        {
            UserDataService.EnsureCreated(Context.User, Context.Guild).Wait();
            base.BeforeExecute(command);
        }
    }
}