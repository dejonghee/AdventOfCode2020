using System.Collections.Generic;

namespace AdventOfCode.Core.Utils
{
    public static class HashSetExtensions
    {
        public static HashSet<T> TryAddRange<T>(this HashSet<T> hashSet, IEnumerable<T> toAdd)
        {
            if (hashSet == null)
                return null;
            if (toAdd == null)
                return hashSet;

            foreach (var item in toAdd)
            {
                hashSet.Add(item);
            }

            return hashSet;
        }
    }
}
