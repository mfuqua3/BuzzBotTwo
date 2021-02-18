using System.Collections.Generic;

namespace BuzzBotTwo.Domain.Seed
{
    public interface IDataSeeder<out T> where T : class
    {
        IEnumerable<T> Data { get; }
    }
}