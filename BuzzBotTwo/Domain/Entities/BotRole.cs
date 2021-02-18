using BuzzBotTwo.Domain.Entities;

namespace BuzzBotTwo.Domain
{
    public class BotRole:IEntity<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public enum BotRoleLevel
    {
        Owner = 1,
        Admin = 2,
        User = 5
    }
}