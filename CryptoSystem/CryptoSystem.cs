namespace CryptoSystem
{
    /// <include file='documentation.xml' path='docs/members[@name="CryptoSystem"]/CryptoSystem/*'/>
    public abstract class CryptoSystem
    {
        public CryptoSystem(string userKeyPath)
        {
            KeyPath = userKeyPath;
        }      
        
        /// <include file='documentation.xml' path='docs/members[@name="CryptoSystem"]/KeyPath/*'/>
        public abstract string KeyPath { set; }

        /// <include file='documentation.xml' path='docs/members[@name="CryptoSystem"]/Decrypt/*'/>
        public abstract void Decrypt(string inputPath, string outputPath);

        /// <include file='documentation.xml' path='docs/members[@name="CryptoSystem"]/Encrypt/*'/>
        public abstract void Encrypt(string inputPath, string outputPath);
    }
}
