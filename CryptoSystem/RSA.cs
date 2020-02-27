using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSystem
{
    class RSA : CryptoSystem
    {
        public event Action RsaNotify;
        public event Action ProgressNotify;

        private string rsaKeyPath;
        public RSA(string userKey) : base(userKey) {}
        public override string KeyPath
        {
            set
            {
                rsaKeyPath = value;
            }
        }
        public override void Decrypt(string inputPath, string outputPath)
        {
            var file = ReadFile(inputPath);

            var _d = RsaKeyManager.GetPartKey(rsaKeyPath, 0);
            var _n = RsaKeyManager.GetPartKey(rsaKeyPath, 1);

            var sizePart = GetPartKeyLength(_n);

            var byteList = new List<byte>();
            var tempByteList = new List<byte>();

            int fullSize = file.Length / sizePart;
            int elementalPart = fullSize / 100;

            for (int i = 0; i < fullSize; i++)
            {
                var l = 0;
                for (int j = sizePart - 1; j >= 0; j--)
                {
                    if (file[i * sizePart + j] != 0)
                    {
                        l = j; 
                        break;
                    }
                }
                for (int j = 0; j < l + 1; j++)
                {
                    tempByteList.Add(file[i * sizePart + j]);
                }
                if (tempByteList[tempByteList.Count - 1] > 127) tempByteList.Add(0);
                var m = BigInteger.ModPow(new BigInteger(tempByteList.ToArray()), _d, _n).ToByteArray();
                byteList.Add(m[0]);
                tempByteList.Clear();

                if (elementalPart != 0 && i % elementalPart == 0) ProgressNotify?.Invoke();
            }

            File.WriteAllBytes(outputPath, byteList.ToArray());
        }

        private int GetPartKeyLength(BigInteger keyPart)
        {
            int leng = keyPart.ToByteArray().Length; 
            var sizePart = 0;
            if ((Math.Log(leng, 2) % 1) == 0) sizePart = leng;
            else sizePart = leng + 1;
            return sizePart;
        }

        private byte[] ReadFile(string filePath)
        {            
            byte[] byteArray;
            using (FileStream fstream = File.OpenRead(filePath))
            {
                byteArray = new byte[fstream.Length];
                fstream.Read(byteArray, 0, byteArray.Length);
            }
            return byteArray;
        }        

        public override void Encrypt(string inputPath, string outputPath)
        {
           
            byte[] file = ReadFile(inputPath);

            var _e = RsaKeyManager.GetPartKey(rsaKeyPath, 0);
            var _n = RsaKeyManager.GetPartKey(rsaKeyPath, 1);

            var sizePart = GetPartKeyLength(_n);

            var byteList = new List<byte>();

            int fullSize = file.Length;
            int elementalPart = fullSize / 100;

            for (int i = 0; i < fullSize; i++)
            {
                var c = BigInteger.ModPow(new BigInteger(file[i]), _e, _n).ToByteArray();
                byteList.AddRange(c);
                for (int j = 0; j < sizePart - c.Length; j++)
                {
                    byteList.Add(0);
                }
                if (elementalPart != 0 && (i+1) % elementalPart == 0) ProgressNotify?.Invoke();
            }

            File.WriteAllBytes(outputPath, byteList.ToArray());
        }

        public async void EncryptAsync(string inputPath, string outputPath)
        {
            await Task.Run(() => Encrypt(inputPath, outputPath));
            RsaNotify?.Invoke();
        }

        public async void DecryptAsync(string inputPath, string outputPath)
        {
            await Task.Run(() => Decrypt(inputPath, outputPath));
            RsaNotify?.Invoke();
        }
    }
}
