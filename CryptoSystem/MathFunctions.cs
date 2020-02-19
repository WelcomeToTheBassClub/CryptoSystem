using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSystem
{
    static class MathFunctions
    {
        static private Random Randomizer = new Random();
        static public BigInteger GetNumber(int size)
        {
            byte[] firstPart = new byte[size - 1];
            Randomizer.NextBytes(firstPart);

            byte[] secondPart = new byte[1];
            secondPart[0] = (byte)Randomizer.Next(0, 128);

            var byteList = new List<byte>(firstPart.Concat(secondPart));
            var number = new BigInteger(byteList.ToArray());
            return number;
        }

        public static BigInteger Inverse(BigInteger a, BigInteger b) //Euclidean algorithm
        {
            BigInteger x, y, x1, x2, y1, y2, q, r;
            BigInteger temp = b;
            x1 = 0; x2 = 1; y1 = 1; y2 = 0;
            while (b > 0)
            {
                q = a / b;
                r = a - q * b;
                x = x2 - q * x1;
                y = y2 - q * y1;
                a = b; b = r;
                x2 = x1;
                x1 = x;
                y2 = y1;
                y1 = y;
            }
            if (x2 < 0) x2 = temp + x2;
            return x2;
        }

        static public bool CheckNumberPrimality(int countRounds, BigInteger number, int size) //Miller Rabin Test
        {
            BigInteger s = 0;
            BigInteger t = number - 1;
            while (t % 2 == 0)
            {
                t = t / 2;
                s++;
            }
            for (int i = 0; i < countRounds; i++)
            {
                BigInteger a = GetNumber(size);
                while (a >= number) a = GetNumber(size);

                var x = BigInteger.ModPow(a, t, number);
                if (x == 1 || x == number - 1) continue;
                for (int j = 0; j < s - 1; j++)
                {
                    x = BigInteger.ModPow(x, 2, number);
                    if (x == 1) return false;
                    if (x == number - 1)
                    {
                        goto l;
                    }
                }
                return false;
            l:;
            }
            return true;
        }
    }
}
