using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace CryptoSystem
{
    enum KeyType
    {
        Public = 0,
        Private = 1
    }
    enum PartNumber
    {
        First = 0,
        Second = 1
    }

    /// <include file='documentation.xml' path='docs/members[@name="RsaKeyManager"]/RsaKeyManager/*'/>
    public class RsaKeyManager
    {
        #region Key Parameters
        /// <include file='documentation.xml' path='docs/members[@name="RsaKeyManager"]/KMNotify/*'/>
        public event Action<string, string> KMNotify;
        /// <include file='documentation.xml' path='docs/members[@name="RsaKeyManager"]/Size/*'/>
        private int Size { get; set; }
        /// <include file='documentation.xml' path='docs/members[@name="RsaKeyManager"]/numberN/*'/>
        private BigInteger NumberN { get; set; }
        /// <include file='documentation.xml' path='docs/members[@name="RsaKeyManager"]/numberE/*'/>
        private BigInteger NumberE { get; set; }
        /// <include file='documentation.xml' path='docs/members[@name="RsaKeyManager"]/numberD/*'/>
        private BigInteger NumberD { get; set; }
        #endregion 

        public RsaKeyManager(int size)
        {
            Size = size;
            NumberN = NumberD = NumberE = 0;
        }

        /// <include file='documentation.xml' path='docs/members[@name="RsaKeyManager"]/GetNewParams/*'/>
        private void GetNewParams()
        {
            NumberN = NumberD = NumberE = 0;
            BigInteger _p = 0;
            BigInteger _q = 0;
            while (NumberN < 256 || _p == _q)
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
                NumberN = _p * _q;
            }

            var funcResult = (_p - 1) * (_q - 1);
            NumberE = BigInteger.Pow(2, (int)Math.Pow(2, Math.Log(Size, 2))) + 1;
            NumberD = MathFunctions.Inverse(NumberE, funcResult);

            if ((NumberE * NumberD) % funcResult != 1)
                GetNewParams();
        }

        /// <include file='documentation.xml' path='docs/members[@name="RsaKeyManager"]/SaveKeysAsync/*'/>
        public async void SaveKeysAsync(string publicPath, string privatePath)
        {
            await Task.Run(() => SaveKeys(publicPath, privatePath));
            KMNotify?.Invoke(publicPath, privatePath);
        }

        /// <include file='documentation.xml' path='docs/members[@name="RsaKeyManager"]/SaveKeys/*'/>
        public void SaveKeys(string publicPath, string privatePath)
        {
            GetNewParams();
            
            WriteKey(publicPath, KeyType.Public);
            WriteKey(privatePath, KeyType.Private);           
        }

        /// <include file='documentation.xml' path='docs/members[@name="RsaKeyManager"]/WriteKey/*'/>
        private void WriteKey(string filePath, KeyType type)
        {
            RsaKey key;
            using (var fStream = new FileStream(filePath, FileMode.Create))
            {
                BigInteger firstNumber = 0;

                if ((int)type == 0) 
                    key = new RsaKey(NumberE, NumberN);
                else 
                    key = new RsaKey(NumberD, NumberN);

                var fileKay = BuildKey(key);
                fStream.Write(fileKay, 0, fileKay.Length);
            }
        }

        /// <include file='documentation.xml' path='docs/members[@name="RsaKeyManager"]/BuildKey/*'/>
        private byte[] BuildKey(RsaKey key)
        {
            byte[] buffer1 = key.FirstPart.ToByteArray();
            byte[] buffer2 = key.SecondPart.ToByteArray();

            int sizePart = 0;
            if ((Math.Log(buffer2.Length, 2) % 1) == 0)
                sizePart = buffer2.Length;           
            else
                sizePart = buffer2.Length + 1;

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

        /// <include file='documentation.xml' path='docs/members[@name="RsaKeyManager"]/GetRsaKey/*'/>
        static public RsaKey GetRsaKey(string path)
        {
            byte[] file;
            using (FileStream fstream = File.OpenRead(path))
            {
                file = new byte[fstream.Length];
                fstream.Read(file, 0, file.Length);
            }
            var sizePart = file.Length / 2; //получаем размеры частей ключа
            var length = 0;
            for (int i = sizePart - 1; i >= 0; i--)  //вычисляем размер части без последних нулей
            {
                if (file[sizePart + i] != 0)
                    length = i; 
                break;             
            }

            byte[] firstPartArray = GetPartKey(ref file, sizePart, length, PartNumber.First);
            byte[] secondPartArray = GetPartKey(ref file, sizePart, length, PartNumber.Second); 

            var firstPart = new BigInteger(firstPartArray);
            var secondPart = new BigInteger(secondPartArray);

            return new RsaKey(firstPart, secondPart);
        }

        static private byte[] GetPartKey(ref byte[] fileKey, int sizePart, int length, PartNumber partNumber)
        {
            byte[] keyPartArray;

            if (fileKey[sizePart + length] > 127)
                keyPartArray = new byte[length + 2];
            else
                keyPartArray = new byte[length + 1];

            Array.Copy(fileKey, sizePart * (int)partNumber, keyPartArray, 0, length + 1);
            return keyPartArray;
        }
    }
}
