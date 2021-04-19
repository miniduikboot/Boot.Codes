using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Boot.Codes
{
    public static class Extensions
    {
        private static readonly Random WeakRng = new Random();

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            var size = list.Count;
            while (size > 1)
            {
                var i = WeakRng.Next(0, size);
                size--;
                var value = list[i];

                list[i] = list[size];
                list[size] = value;
            }

            return list;
        }
    }
}
