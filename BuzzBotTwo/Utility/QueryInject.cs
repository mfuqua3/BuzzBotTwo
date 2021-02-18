using System.Linq;

namespace BuzzBotTwo.Utility
{
    public delegate IQueryable<T> QueryInject<T>(IQueryable<T> query);
}