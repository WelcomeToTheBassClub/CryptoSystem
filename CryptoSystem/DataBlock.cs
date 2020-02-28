using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace CryptoSystem
{
    class DataBlock
    {
        private byte[] infoBytes;
        public BigInteger InfoValue { get; private set; }

        private int infoLength;

        public DataBlock(int size)
        {
            infoLength = size;
            infoBytes = new byte[infoLength];
            infoBytes[infoLength - 1] = 0;
        }
        public DataBlock(byte[] bytes, int size) : this(size)
        {
            GetInfoBytes(bytes);
        }
        public void GetInfoBytes(byte[] bytes)
        {
            Array.Copy(bytes, infoBytes, infoLength - 1);
            CreateValueInfo();
        }
        private void CreateValueInfo()
        {
            InfoValue = new BigInteger(infoBytes);
        }
    }
}
