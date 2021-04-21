using System.Collections;

namespace Boot.Codes
{
    public static class Extensions
    {
        /// <summary>
        /// Implementation of the Fisher-Yates shuffle.
        /// Shuffles the list in place.
        /// </summary>
        public static void Shuffle<T>(this T list) where T : IList
        {
            var size = list.Count;
            for (var i = 0; i < size - 2; i++)
            {
                var j = StrongRandom.Next(i, size);
                var value = list[i];
                list[i] = list[j];
                list[j] = value;
            }
        }
    }
}
