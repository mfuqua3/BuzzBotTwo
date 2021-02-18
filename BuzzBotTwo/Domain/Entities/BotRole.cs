namespace BuzzBotTwo.Domain
{
    public class BotRole
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