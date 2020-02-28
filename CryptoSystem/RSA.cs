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
            
            int sizePart = GetPartKeyLength(_n);

            GetIncreasedBytes(ref file, sizePart);

            int fullSize = file.Length / (sizePart - 1);
            int elementalPart = fullSize / 100;


            DataBlock filePart = new DataBlock(sizePart);

            using (var fStream = new FileStream(outputPath, FileMode.Create))
            {
                for (int i = 1; i < file.Length + 1; i++)
                {
                    if (i % (sizePart) == 0)
                    {
                        byte[] tempBytes = new byte[sizePart];
                        Array.Copy(file, i - sizePart, tempBytes, 0, sizePart);
                        var secretMessage = new BigInteger(tempBytes);
                        var decryptedMessage = BigInteger.ModPow(secretMessage, _d, _n).ToByteArray();
                        GetIncreasedBytes(ref decryptedMessage, sizePart);

                        byte[] resultMessage = new byte[sizePart - 1];
                        Array.Copy(decryptedMessage, 0, resultMessage, 0, sizePart - 1);

                        if (i == file.Length)
                        {
                            byte[] lastResultMessage = new byte[sizePart - GetLastZeroCount(resultMessage)];
                            Array.Copy(resultMessage, 0, lastResultMessage, 0, lastResultMessage.Length - 1);
                            fStream.Write(lastResultMessage, 0, lastResultMessage.Length - 1);
                        }
                        else
                        {
                            fStream.Write(resultMessage, 0, sizePart - 1);
                        }
                        if (elementalPart != 0 && (i + 1) % elementalPart == 0) ProgressNotify?.Invoke();
                    }
                }
            }
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

            int sizePart = GetPartKeyLength(_n);     

            GetIncreasedBytes(ref file, sizePart-1);
            int fullSize = file.Length / (sizePart - 1);
            int elementalPart = fullSize / 100;

            DataBlock filePart = new DataBlock(sizePart);

            using (var fStream = new FileStream(outputPath, FileMode.Create))
            {
                for (int i = 1; i < file.Length + 1; i++)
                {
                    if (i % (sizePart-1) == 0)
                    {
                        byte[] tempBytes = new byte[sizePart - 1];
                        Array.Copy(file, i - (sizePart - 1), tempBytes, 0, sizePart - 1);
                        filePart.GetInfoBytes(tempBytes);
                        var message = filePart.InfoValue;
                        byte[] encryptedMessage = BigInteger.ModPow(message, _e, _n).ToByteArray();
                        GetIncreasedBytes(ref encryptedMessage, sizePart);                   
                        fStream.Write(encryptedMessage, 0, sizePart);

                        if (elementalPart != 0 && (i + 1) % elementalPart == 0) ProgressNotify?.Invoke();
                    }
                }
            }
        }


        private void GetIncreasedBytes(ref byte[] byteArray, int size)
        {
            int difference = size - (byteArray.Length % size);
            
            if (difference < size)
            {
                byte[] tempBytes = new byte[byteArray.Length + difference];
                Array.Copy(byteArray, tempBytes, byteArray.Length);
                byteArray = tempBytes;
            }
        }

        private int GetLastZeroCount(byte[] bytes)
        {
            int count = 0;
            for (int l = bytes.Length - 1; l > 0; l--)
            {
                if (bytes[l] == 0) count++;
                else break;
            }
            return count;
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
