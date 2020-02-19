using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSystem
{
    enum KeyType
    {
        Public = 0,
        Private = 1
    }
    class RsaKeyManager
    {
        public event Action<string, string> KMNotify;

        private int Size { get; set; }

        private BigInteger numberN { get; set; }
        private BigInteger numberE { get; set; }
        private BigInteger numberD { get; set; }

        public RsaKeyManager(int size)
        {
            Size = size;
            numberN = numberD = numberE = 0;
        }

        private void GetNewParams()
        {
            numberN = numberD = numberE = 0;
            BigInteger _p = 0;
            BigInteger _q = 0;
            while (numberN < 256 || _p == _q)
            {
                int count = 0;
                while (count != 2)
                {
                    BigInteger temp = MathFunctions.GetNumber(Size);
                    if (temp % 2 != 0 && temp > 1)
                    {
                        if (MathFunctions.CheckNumberPrimality(5, temp, Size))
                        {
                            count++;
                            if (count == 1) _p = temp;
                            if (count == 2) _q = temp;
                        }
                    }

                }
                numberN = _p * _q;
            }

            var funcResult = (_p - 1) * (_q - 1);

            //(2^2^n)+1 для ключа 32(256=2^8) байт максимальное n=8, для 64 = 9, для 128 = 10 и тд 
            numberE = BigInteger.Pow(2, (int)Math.Pow(2, Math.Log(Size, 2))) + 1;
            numberD = MathFunctions.Inverse(numberE, funcResult);

            if ((numberE * numberD) % funcResult != 1) GetNewParams();
        }

        public async void SaveKeysAsync(string publicPath, string privatePath)
        {
            await Task.Run(() => SaveKeys(publicPath, privatePath));
            KMNotify?.Invoke(publicPath, privatePath);
        }

        public void SaveKeys(string publicPath, string privatePath)
        {
            GetNewParams();
            
            WriteKey(publicPath, KeyType.Public);
            WriteKey(privatePath, KeyType.Private);           
        }

        private void WriteKey(string filePath, KeyType type)
        {
            using (var fstream = new FileStream(filePath, FileMode.Create))
            {
                BigInteger firstNumber = 0;
                if ((int)type == 0) 
                {
                    firstNumber = numberE;
                }
                else firstNumber = numberD;
                var tempKey = CreateKey(firstNumber, numberN);
                fstream.Write(tempKey, 0, tempKey.Length);
            }
        }

        private byte[] CreateKey(BigInteger firstNumber, BigInteger secondNumber)
        {
            byte[] buffer1 = firstNumber.ToByteArray();
            byte[] buffer2 = secondNumber.ToByteArray();

            int sizePart = 0;
            if ((Math.Log(buffer2.Length, 2) % 1) == 0)
            {
                sizePart = buffer2.Length;
            }
            else sizePart = buffer2.Length + 1;

            var beginList = new List<byte>(buffer1);
            for (int i = 0; i < sizePart - buffer1.Length; i++)
            {
                beginList.Add(0);
            }

            var endList = new List<byte>(buffer2);
            for (int i = 0; i < sizePart - buffer2.Length; i++)
            {
                endList.Add(0);
            }

            var result = beginList.Concat(endList);

            return result.ToArray();
        }

        static public BigInteger GetPartKey(string path, int part) //получение необходимой части ключа
        {
            byte[] file;
            using (FileStream fstream = File.OpenRead(path))
            {
                file = new byte[fstream.Length];
                fstream.Read(file, 0, file.Length);
            }

            var sizePart = file.Length / 2; //получаем размеры частей ключа

            var byteList = new List<byte>();
            var length = 0;

            for (int i = sizePart - 1; i >= 0; i--)  //вычисляем размер части без последних нулей
            {
                if (file[sizePart * part + i] != 0)
                {
                    length = i; break;
                }
            }
            for (int j = 0; j < length + 1; j++)
            {
                byteList.Add(file[sizePart * part + j]); //записываем необходимую часть ключа
            }
            if (byteList[byteList.Count - 1] > 127) byteList.Add(0); //добавляем 0, если последний байт "отрицательный"

            return new BigInteger(byteList.ToArray());
        }
    }
}
