using System;
using System.Collections.Generic;

namespace Boot.Codes
{
    public static class Extensions
    {
        private static readonly Random WeakRng = new Random();

        /**
         * Shuffle the array in-place.
         */
        public static void Shuffle<T>(this IList<T> list)
        {
            // Implementation of the Fisher-Yates shuffle
            var size = list.Count;
            for (var i = 0; i < size - 2; i++)
            {
                var j = WeakRng.Next(i, size);
                var value = list[i];
                list[i] = list[j];
                list[j] = value;
            }
        }
    }
}
