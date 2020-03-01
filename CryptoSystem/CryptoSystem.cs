namespace CryptoSystem
{
    /// <summary>
    /// Класс, переопределяющий основные функции и параметры, необходимые для реализации криптографической системы.
    /// </summary>
    public abstract class CryptoSystem
    {
        public CryptoSystem(string userKeyPath)
        {
            KeyPath = userKeyPath;
        }      
        
        /// <summary>
        /// Полный путь к криптографическому ключу.
        /// </summary>
        public abstract string KeyPath { set; }

        /// <summary>
        /// Производит обратное криптографическое преобразование файла, 
        /// расположенного по пути <paramref name="inputPath"/> и сохраняет результат 
        /// по пути <paramref name="outputPath"/>.
        /// </summary>
        /// <param name="inputPath">Путь к файлу, который необходимо расшифровать.</param>
        /// <param name="outputPath">Путь по которому необходимо сохранить расшифрованный файл.</param>
        public abstract void Decrypt(string inputPath, string outputPath);

        /// <summary>
        /// Производит криптографическое преобразование файла, 
        /// расположенного по пути <paramref name="inputPath"/> и сохраняет результат 
        /// по пути <paramref name="outputPath"/>.
        /// </summary>
        /// <param name="inputPath">Путь к файлу, который необходимо зашифровать.</param>
        /// <param name="outputPath">Путь по которому необходимо сохранить зашифрованный файл.</param>
        public abstract void Encrypt(string inputPath, string outputPath);
    }
}
