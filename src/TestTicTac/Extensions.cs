using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace TestTicTac
{
    public static class Extensions
    {

        // use good RNG to shuffle a List
        public static void Shuffle<T>(this IList<T> list)
        {
            var rnd = RandomNumberGenerator.Create();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do rnd.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }


        public static double GetDouble(this RandomNumberGenerator r)
        {
            // Step 1: fill an array with 8 random bytes
            var bytes = new Byte[8];
            r.GetBytes(bytes);
            // Step 2: bit-shift 11 and 53 based on double's mantissa bits
            var ul = BitConverter.ToUInt64(bytes, 0) / (1 << 11);
            Double d = ul / (Double)(1UL << 53);
            return d;
        }

    }
}
