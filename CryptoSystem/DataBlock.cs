using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace CryptoSystem
{
    /// <include file='documentation.xml' path='docs/members[@name="DataBlock"]/DataBlock/*'/>
    class DataBlock
    {
        private byte[] infoBytes;

        /// <include file='documentation.xml' path='docs/members[@name="DataBlock"]/InfoValue/*'/>
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
            SetInfoBytes(bytes);
        }

        /// <include file='documentation.xml' path='docs/members[@name="DataBlock"]/SetInfoBytes/*'/>
        public void SetInfoBytes(byte[] bytes)
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
