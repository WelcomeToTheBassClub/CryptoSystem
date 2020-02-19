using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSystem
{
    abstract class CryptoSystem
    {
        public CryptoSystem(string userKeyPath)
        {
            KeyPath = userKeyPath;
        }
        public abstract string KeyPath { set; }

        public abstract void Decrypt(string input, string output);

        public abstract void Encrypt(string input, string output);
    }
}
