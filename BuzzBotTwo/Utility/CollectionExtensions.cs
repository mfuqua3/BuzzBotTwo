using System;
using System.Collections.Generic;

namespace BuzzBotTwo.Utility
{
    public static class CollectionExtensions
    {
        public static IEnumerable<List<T>> SplitList<T>(this List<T> collection, int nSize)
        {
            for (var i = 0; i < collection.Count; i += nSize)
            {
                yield return collection.GetRange(i, Math.Min(nSize, collection.Count - i));
            }
        }
    }
}