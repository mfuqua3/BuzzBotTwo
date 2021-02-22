using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BuzzBotTwo.Utility
{
    public delegate IQueryable<T> QueryInject<T>(IQueryable<T> query, DbContext context = null);
}