using System;
using System.Numerics;

namespace CryptoSystem
{
    /// <summary>
    /// Класс <с>DataBlock</с> представляет блок информации, которым можно 
    /// оперировать в криптографической системе.
    /// </summary>
    public class DataBlock
    {
        private byte[] infoBytes;

        /// <summary>
        /// Возвращает представление блока информации в виде положительного 
        /// <c><see cref="BigInteger"/></c> числа.
        /// </summary>
        public BigInteger InfoValue { get; private set; }

        private int infoLength;

        /// <summary>
        /// Конструктор класса <c><see cref="DataBlock"/></c>, требует размер для объявления.
        /// </summary>
        /// <param name="size"></param>
        public DataBlock(int size)
        {
            infoLength = size;
            infoBytes = new byte[infoLength];
            infoBytes[infoLength - 1] = 0;
        }

        /// <summary>
        /// Передает массив байт <paramref name="bytes"/> в <c><see cref="DataBlock"/></c> 
        /// для его представления в виде блока информации.
        /// </summary>
        /// <param name="bytes">Последовательность байт.</param>
        public void SetInfoBytes(byte[] bytes)
        {
            Array.Copy(bytes, infoBytes, infoLength - 1);
            InfoValue = new BigInteger(infoBytes);
        }
    }
}
