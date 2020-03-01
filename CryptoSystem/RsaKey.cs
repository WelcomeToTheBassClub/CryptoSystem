using System;
using System.Numerics;

namespace CryptoSystem
{
    /// <summary>
    /// Класс <с>RsaKey</с> представляет криптографический ключ для криптосистемы <c><see cref="RSA"/></c>.
    /// </summary>
    public class RsaKey
    {
        /// <summary>
        /// Возвращает первую часть ключа, называемую экспонентой.
        /// </summary>
        public BigInteger FirstPart { get; private set; }

        /// <summary>
        /// Возвращает вторую часть ключа, называемую модулем.
        /// </summary>
        public BigInteger SecondPart { get; private set; }

        /// <summary>
        /// Конструктор класса <c><see cref="RsaKey"/></c>, требует экспоненту и модуль для объявления.
        /// </summary>
        /// <param name="firstPart">Первая часть ключа - экспонента.</param>
        /// <param name="secondPart">Вторая часть ключа - модуль.</param>
        public RsaKey(BigInteger firstPart, BigInteger secondPart)
        {
            if (firstPart < 1 || secondPart < 1)
                throw new ArgumentException();

            FirstPart = firstPart;
            SecondPart = secondPart;
        }
    }
}
