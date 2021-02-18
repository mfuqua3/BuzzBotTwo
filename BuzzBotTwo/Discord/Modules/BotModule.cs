using BuzzBotTwo.Discord.Services;
using Discord.Commands;

namespace BuzzBotTwo.Discord.Modules
{
    public class BotModule : ModuleBase
    {
        private readonly IUserDataService _userDataService;

        public BotModule(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }
        protected override void BeforeExecute(CommandInfo command)
        {
            _userDataService.EnsureCreated(Context.User, Context.Guild).Wait();
            base.BeforeExecute(command);
        }
    }
}