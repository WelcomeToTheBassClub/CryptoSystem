using System;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;

namespace CryptoSystem
{
    /// <summary>
    /// Класс <с>RSA</с> реализует ассиметричную криптографическую систему с помощью ключей,
    /// генерируемых классом <c><see cref="RsaKeyManager"/></c>.
    /// </summary>
    public class RSA : CryptoSystem
    {
        public event Action<string> RsaNotify;
        public event Action<int> ProgressNotify;
        private string rsaKeyPath;
        
        /// <summary>
        /// Конструктор класса <c><see cref="RSA"/></c>, требует полный путь к ключу для объявления.
        /// </summary>
        /// <param name="userKey">Полный путь к криптографическому ключу.</param>
        public RSA(string userKey) : base(userKey) { }
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

            RsaKey decryptKey = RsaKeyManager.GetRsaKey(rsaKeyPath);
            BigInteger _d = decryptKey.FirstPart;
            BigInteger _n = decryptKey.SecondPart;
            int sizePart = GetPartKeyLength(_n);

            IncreaseByteArray(ref file, sizePart);
            RunDecrypt(outputPath, file.Length, sizePart, ref file, _d, _n);
        }

        private void RunDecrypt(string outputPath, int fileLength, int sizePart, ref byte[] file, BigInteger _d, BigInteger _n)
        {
            double elementalPart = fileLength / sizePart / 100.0;
            int elemPartCeil = (int)Math.Ceiling(elementalPart);
            using (var fStream = new FileStream(outputPath, FileMode.Create))
            {        
                int blockCount = 0;
                for (int i = 1; i < file.Length + 1; i++)
                {                    
                    if (i % (sizePart) == 0)
                    {
                        blockCount++;
                        var resultMessage = GetDecryptFunctionResult(sizePart, ref file, i, _d, _n);
                        if (i == file.Length)
                        {
                            byte[] lastResultMessage = new byte[sizePart - GetLastZeroCount(resultMessage)];
                            Array.Copy(resultMessage, 0, lastResultMessage, 0, lastResultMessage.Length - 1);
                            fStream.Write(lastResultMessage, 0, lastResultMessage.Length - 1);
                        }
                        else
                            fStream.Write(resultMessage, 0, sizePart - 1);

                        IncProgress(blockCount, elementalPart, elemPartCeil);
                    }
                }
            }
        }

        private byte[] GetDecryptFunctionResult(int sizePart, ref byte[] file, int index, BigInteger d, BigInteger n)
        {
            byte[] tempBytes = new byte[sizePart];
            Array.Copy(file, index - sizePart, tempBytes, 0, sizePart);
            var secretMessage = new BigInteger(tempBytes);
            var decryptedMessage = BigInteger.ModPow(secretMessage, d, n).ToByteArray();
            IncreaseByteArray(ref decryptedMessage, sizePart);
            byte[] resultMessage = new byte[sizePart - 1];
            Array.Copy(decryptedMessage, 0, resultMessage, 0, sizePart - 1);
            return resultMessage;
        }

        private void IncProgress(int blockCount, double elementalPart, int elemPartCeil)
        {
            if (elementalPart < 1)
            {
                ProgressNotify?.Invoke((int)(blockCount / elementalPart));
            }
            else
            {
                if (blockCount % elemPartCeil == 0)
                {
                    ProgressNotify?.Invoke(blockCount / elemPartCeil);
                }
            }
        }
        
        private int GetPartKeyLength(BigInteger keyPart)
        {
            int leng = keyPart.ToByteArray().Length;
            var sizePart = 0;

            if ((Math.Log(leng, 2) % 1) == 0) 
                sizePart = leng;
            else 
                sizePart = leng + 1;
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

            RsaKey encryptKey = RsaKeyManager.GetRsaKey(rsaKeyPath);
            BigInteger _e = encryptKey.FirstPart;
            BigInteger _n = encryptKey.SecondPart;
            int sizePart = GetPartKeyLength(_n);

            IncreaseByteArray(ref file, sizePart - 1);
            RunEncrypt(outputPath, file.Length, sizePart, ref file, _e, _n);
        }

        private void RunEncrypt(string outputPath, int fileLength, int sizePart, ref byte[] file, BigInteger e, BigInteger n)
        {
            DataBlock filePart = new DataBlock(sizePart);
            double elementalPart = fileLength / (sizePart - 1) / 100.0;
            int elemPartCeil = (int)Math.Ceiling(elementalPart);

            using (var fStream = new FileStream(outputPath, FileMode.Create))
            {
                int blockCount = 0;
                for (int i = 1; i < file.Length + 1; i++)
                {
                    if (i % (sizePart - 1) == 0)
                    {
                        blockCount++;
                        byte[] tempBytes = new byte[sizePart - 1];
                        Array.Copy(file, i - (sizePart - 1), tempBytes, 0, sizePart - 1);
                        filePart.SetInfoBytes(tempBytes);
                        var message = filePart.InfoValue;
                        byte[] encryptedMessage = BigInteger.ModPow(message, e, n).ToByteArray();
                        IncreaseByteArray(ref encryptedMessage, sizePart);
                        fStream.Write(encryptedMessage, 0, sizePart);
                        IncProgress(blockCount, elementalPart, elemPartCeil);
                    }
                }
            }
        }

        private void IncreaseByteArray(ref byte[] byteArray, int size)
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
                if (bytes[l] == 0)
                    count++;
                else
                        break;
            }
            return count;           
        }

        /// <summary>
        /// Производит криптографическое преобразование файла в асинхронном режиме, 
        /// расположенного по пути <paramref name="inputPath"/> и сохраняет результат 
        /// по пути <paramref name="outputPath"/>.
        /// </summary>
        /// <inheritdoc cref="Encrypt">
        /// <param name="inputPath"><inheritdoc/></param>
        /// <param name="outputPath"><inheritdoc/></param>
        /// </inheritdoc>
        public async void EncryptAsync(string inputPath, string outputPath)
        {
            await Task.Run(() => Encrypt(inputPath, outputPath));
            RsaNotify?.Invoke(outputPath);
        }

        /// <summary>
        /// Производит обратное криптографическое преобразование файла в асинхронном режиме, 
        /// расположенного по пути <paramref name="inputPath"/> и сохраняет результат 
        /// по пути <paramref name="outputPath"/>.
        /// </summary>
        /// <inheritdoc cref="Encrypt">
        /// <param name="inputPath"><inheritdoc/></param>
        /// <param name="outputPath"><inheritdoc/></param>
        /// </inheritdoc>
        public async void DecryptAsync(string inputPath, string outputPath)
        {
            await Task.Run(() => Decrypt(inputPath, outputPath));
            RsaNotify?.Invoke(outputPath);
        }
    }
}
