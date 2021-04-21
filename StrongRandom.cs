using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Boot.Codes
{
    public static class StrongRandom
    {
        private static readonly RNGCryptoServiceProvider CryptoRng = new RNGCryptoServiceProvider();

        public static int Next(int minValue, int maxExclusiveValue)
        {
            if (minValue >= maxExclusiveValue) throw new ArgumentOutOfRangeException("minValue must be lower than maxExclusiveValue");

            var difference = (long)maxExclusiveValue - minValue;
            var upperBound = uint.MaxValue / difference * difference;
            uint ui;

            do ui = GetUint();
            while (ui >= upperBound);

            return (int)(minValue + (ui % difference));
        }

        private static uint GetUint()
        {
            var buffer = new byte[4];
            CryptoRng.GetBytes(buffer);
            return BitConverter.ToUInt32(buffer);
        }
    }
}
