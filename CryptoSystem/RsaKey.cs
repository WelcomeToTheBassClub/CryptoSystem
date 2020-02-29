﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace CryptoSystem
{
    /// <include file='documentation.xml' path='docs/members[@name="RsaKey"]/RsaKey/*'/>
    class RsaKey
    {
        public BigInteger FirstPart { get; private set; }
        public BigInteger SecondPart { get; private set; }

        public RsaKey(BigInteger firstPart, BigInteger secondPart)
        {
            if (firstPart < 1 || secondPart < 1) throw new ArgumentException();
            FirstPart = firstPart;
            SecondPart = secondPart;
        }
    }
}