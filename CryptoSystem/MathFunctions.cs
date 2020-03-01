using System;
using System.Numerics;

namespace CryptoSystem
{
    /// <summary>
    /// Класс <с>MathFunctions</с> реализует математические алгоритмы, необходимые 
    /// для генерации ключей шифрования. 
    /// </summary>
    public static class MathFunctions
    {
        private static Random Randomizer = new Random();

        /// <summary>
        /// Генерирует случайное положительное <c><see cref="BigInteger"/></c> число размера <paramref name="size"/>.
        /// </summary>
        /// <param name="size">Размер возвращаемого числа в байтах.</param>
        /// <returns>Положительное <c><see cref="BigInteger"/></c> число.</returns>
        public static BigInteger GetNumber(int size)
        {
            byte[] firstPart = new byte[size - 1];
            Randomizer.NextBytes(firstPart);

            byte[] secondPart = new byte[1];
            secondPart[0] = (byte)Randomizer.Next(0, 128);

            byte[] result = new byte[size];
            Array.Copy(firstPart, result, size - 1);
            Array.Copy(secondPart, 0, result, firstPart.Length, 1);

            var number = new BigInteger(result);
            return number;
        }

        /// <summary>
        /// Возвращает обратное число к <paramref name="a"/> по модулю <paramref name="b"/>.
        /// </summary>
        /// <param name="a">Число, обратное к которому необходимо вычислить.</param>
        /// <param name="b">Модуль, по которому необходимо вычислить обратное.</param>
        /// <returns><c><see cref="BigInteger"/></c> число.</returns>
        public static BigInteger Inverse(BigInteger a, BigInteger b) 
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
            if (x2 < 0) 
                x2 = temp + x2;
            return x2;
        }

        /// <summary>
        /// Осуществляет проверку на простоту числа <paramref name="number"/> размера <paramref name="size"/>.
        /// </summary>
        /// <param name="roundsCount">Число раундов теста.</param>
        /// <param name="number">Проверяемое число.</param>
        /// <param name="size">Максимально возможный размер проверяемого числа в байтах.</param>
        /// <returns>Возможные результаты проверки: true - число простое, false - число составное.</returns>
        public static bool CheckNumberPrimality(int roundsCount, BigInteger number, int size)
        {
            int s = 0;
            BigInteger t = number - 1;
            while (t % 2 == 0)
            {
                t = t / 2;
                s++;
            }
            for (int i = 0; i < roundsCount; i++)
            {
                BigInteger a = GetNumber(size);
                while (a >= number)
                    a = GetNumber(size);

                var x = BigInteger.ModPow(a, t, number);
                if (x == 1 || x == number - 1) 
                    continue;
                if (s < 2) 
                    return false;

                for (int j = 0; j < s - 1; j++)
                {
                    x = BigInteger.ModPow(x, 2, number);
                    if (x == 1) 
                        return false;
                    if (x == number - 1)
                        break;
                    if (j == s - 2) 
                        return false;
                }
            }
            return true;
        }
    }
}
