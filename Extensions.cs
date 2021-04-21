using System.Collections;

namespace Boot.Codes
{
    public static class Extensions
    {
        /**
         * Shuffle the array in-place.
         */
        public static T Shuffle<T>(this T list) where T : IList
        {
            // Implementation of the Fisher-Yates shuffle
            var size = list.Count;
            for (var i = 0; i < size - 2; i++)
            {
                var j = StrongRandom.Next(i, size);
                var value = list[i];
                list[i] = list[j];
                list[j] = value;
            }

            // allows method chaining
            return list;
        }
    }
}
