using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CryptoSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string FilePath { get; set; }
        RSA cryptoMachine;

        private void button1_Click(object sender, EventArgs e)
        {
            string publicKeyPath;
            string outputFilePath;
            try
            {
                if (!Path.IsPathRooted(FilePath) || String.IsNullOrWhiteSpace(FilePath))
                    throw new FormatException("Путь к файлу некорректен или отсутствует");
            }
            catch(FormatException ex) 
            {
                MessageBox.Show(ex.Message, "Произошла ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }        

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Выберите открытый ключ";
            openDialog.Filter = "(*.key)|*.key|All files (*.*)|*.*";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                publicKeyPath = openDialog.FileName;
                cryptoMachine = new RSA(publicKeyPath);
            }
            else return;

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = "Укажите путь сохраняемого файла";
            saveDialog.FileName = "secret_" + Path.GetFileName(FilePath);
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                outputFilePath = saveDialog.FileName;
            }
            else return;

            cryptoMachine.ProgressNotify += IncProgressEncryption;
            cryptoMachine.RsaNotify += ShowEncryptMessage;
            cryptoMachine.EncryptAsync(FilePath, outputFilePath);           
        }

        private void IncProgressEncryption()
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Invoke(new Action(() => progressBar1.Value++));
            }           
        }
        private void IncProgressDecryption()
        {
            if (progressBar2.Value < 100)
            {
                progressBar2.Invoke(new Action(() => progressBar2.Value++));
            }           
        }

        private void ShowSaveKeysMessage(string pathPublicKey, string pathPrivateKey)
        {
            label2.Visible = linkLabel2.Visible = true;
            label3.Visible = linkLabel3.Visible = true;
            linkLabel2.Text = pathPublicKey;
            linkLabel3.Text = pathPrivateKey;

            MessageBox.Show("Ключи успешно записаны");
        }
        private void ShowEncryptMessage()
        {
            MessageBox.Show("Файл успешно зашифрован");
            progressBar1.Value = 0;
        }
        private void ShowDecryptMessage()
        {
            MessageBox.Show("Файл успешно расшифрован");
            progressBar2.Value = 0;
        }

        private void ChooseFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Выберите любой файл";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                FilePath = openDialog.FileName;
                label1.Visible = true;
                linkLabel1.Visible = true;
                linkLabel1.Text = openDialog.SafeFileName;
            }          
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(FilePath);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int sizeKey = int.Parse(textBox1.Text);
            RsaKeyManager keyManager = new RsaKeyManager(sizeKey);

            string publicKeyPath = "";
            string privateKeyPath = "";
          
            try
            {
                ShowSaveKeysDialogs(ref publicKeyPath, ref privateKeyPath);              
            }
            catch(ArgumentNullException ex)
            {
                return;
            }
            keyManager.KMNotify += ShowSaveKeysMessage;
            keyManager.SaveKeysAsync(publicKeyPath, privateKeyPath);

        }

        private void ShowSaveKeysDialogs(ref string publicKeyPath, ref string privateKeyPath)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.FileName = "PublicKey";
            saveDialog.DefaultExt = "key";
            saveDialog.Title = "Укажите место для сохранения открытого ключа";
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                publicKeyPath = saveDialog.FileName;
                saveDialog.FileName = "PrivateKey";
                saveDialog.Title = "Укажите место для сохранения закрытого ключа";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    privateKeyPath = saveDialog.FileName;
                }
                else throw new ArgumentNullException();
            }
            else throw new ArgumentNullException();
        }
       
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = Math.Pow(2,trackBar1.Value).ToString();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer", Path.GetDirectoryName(linkLabel2.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer", Path.GetDirectoryName(linkLabel3.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string privateKeyPath;
            string outputFilePath;
            try
            {
                if (!Path.IsPathRooted(FilePath) || String.IsNullOrWhiteSpace(FilePath))
                    throw new FormatException("Путь к файлу некорректен или отсутствует");
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Произошла ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Выберите закрытый ключ";
            openDialog.Filter = "(*.key)|*.key|All files (*.*)|*.*";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                privateKeyPath = openDialog.FileName;
                cryptoMachine = new RSA(privateKeyPath);
            }
            else return;

            SaveFileDialog saveDialog = new SaveFileDialog();
            
            saveDialog.Title = "Укажите путь сохраняемого файла";
            saveDialog.FileName = "decrypted_" + Path.GetFileName(FilePath);
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                outputFilePath = saveDialog.FileName;
            }
            else return;

            cryptoMachine.ProgressNotify += IncProgressDecryption;
            cryptoMachine.RsaNotify += ShowDecryptMessage;
            cryptoMachine.DecryptAsync(FilePath, outputFilePath);
        }
    }
}
