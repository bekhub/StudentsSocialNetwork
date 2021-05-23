using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Helpers
{
    public static class Numbers
    {
        private static readonly Random Random = new();
        
        public static List<int> GenerateRandoms(int count, int min, int max)
        {
            var candidates = new HashSet<int>();
            while (candidates.Count < count) 
                candidates.Add(Random.Next(min, max));
            return candidates.ToList();
        }
    }
}
