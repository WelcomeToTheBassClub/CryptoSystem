using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CryptoSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {           
            InitializeComponent();
            logBox.ScrollBars = RichTextBoxScrollBars.None;
        }

        private string FilePath { get; set; }
        private RSA cryptoMachine;
       
        private void EncryptButton_Click(object sender, EventArgs e)
        {
            string outputFilePath;
            try
            {
                if (!Path.IsPathRooted(FilePath) || String.IsNullOrWhiteSpace(FilePath))
                    throw new FormatException("Путь к файлу некорректен или отсутствует");
            }
            catch(FormatException ex) 
            {
                WriteActionLog("Файл не удалось зашифровать: " + ex.Message, true);
                return;
            }

            if (ShowChooseKeyDialog("Выберите открытый ключ"))
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Title = "Укажите путь сохраняемого файла";
                    saveDialog.FileName = "secret_" + Path.GetFileName(FilePath);
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                        outputFilePath = saveDialog.FileName;
                    else
                        return;
                }

                progressBar.Value = 0;
                cryptoMachine.ProgressNotify += IncProgress;
                cryptoMachine.RsaNotify += ShowEncryptMessage;
                WriteActionLog($"Началось шифрование файла ", Path.GetFileName(FilePath));
                cryptoMachine.EncryptAsync(FilePath, outputFilePath);
            }                
        }

        private void IncProgress(int val)
        {
            progressBar.Invoke(new Action(() => progressBar.Value = val));          
        }

        private void ShowEncryptMessage(string outputPath)
        {
            WriteActionLog($@"Зашифрованный файл сохранен по адресу: {Path.GetDirectoryName(outputPath)}\", Path.GetFileName(outputPath));
            progressBar.Value = 0;
        }
        private void ShowDecryptMessage(string outputPath)
        {
            WriteActionLog($@"Расшифрованный файл сохранен по адресу: {Path.GetDirectoryName(outputPath)}\", Path.GetFileName(outputPath));
            progressBar.Value = 0;
        }

        private void ChooseFileButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Title = "Выберите любой файл";
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    filePathBox.Text = openDialog.FileName;
                    FilePath = openDialog.FileName;
                    WriteActionLog("Выбран файл ", openDialog.SafeFileName);
                }
            }               
        }

        private void CreateKeysButton_Click(object sender, EventArgs e)
        {
            int sizeKey = int.Parse(keySizeBox.Text);
            RsaKeyManager keyManager = new RsaKeyManager(sizeKey);

            string publicKeyPath = "";
            string privateKeyPath = "";
          
            try
            {
                ShowSaveKeysDialogs(ref publicKeyPath, ref privateKeyPath);              
            }
            catch
            {
                WriteActionLog("Пути сохранения ключей указаны некорректно.", true);
                return;
            }
            keyManager.KMNotify += ShowSaveKeysMessage;
            keyManager.SaveKeysAsync(publicKeyPath, privateKeyPath);
        }
        private void ShowSaveKeysMessage(string publicKeyPath, string privateKeyPath)
        {
            var dirPrivateKey = Path.GetDirectoryName(privateKeyPath);
            var dirPublicKey = Path.GetDirectoryName(publicKeyPath);
            WriteActionLog($@"Открытый ключ сохранен по адресу {dirPublicKey}\", Path.GetFileName(publicKeyPath));
            WriteActionLog($@"Закрытый ключ сохранен по адресу {dirPrivateKey}\", Path.GetFileName(privateKeyPath));
        }

        private void ShowSaveKeysDialogs(ref string publicKeyPath, ref string privateKeyPath)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
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
        }
       
        private void keySizeTrackBar_Scroll(object sender, EventArgs e)
        {
            keySizeBox.Text = Math.Pow(2,keySizeTrackBar.Value).ToString();
        }

        private bool ShowChooseKeyDialog(string dialogTitle)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Title = dialogTitle;
                openDialog.Filter = "(*.key)|*.key|All files (*.*)|*.*";
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    var KeyPath = openDialog.FileName;
                    cryptoMachine = new RSA(KeyPath);

                    var keyInfo = new FileInfo(KeyPath);
                    if (keyInfo.Length > 4096)
                    {
                        WriteActionLog("Выбранный файл скорее всего не являлся ключем.", true);
                        return false;
                    }
                    return true;
                }
                else
                    return false;   
            }
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            string outputFilePath;
            try
            {
                if (!Path.IsPathRooted(FilePath) || String.IsNullOrWhiteSpace(FilePath))
                    throw new FormatException("Путь к файлу некорректен или отсутствует");
            }
            catch (FormatException ex)
            {
                WriteActionLog("Файл не удалось расшифровать: " + ex.Message, true);
                return;
            }

            if (ShowChooseKeyDialog("Выберите закрытый ключ"))
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Title = "Укажите путь сохраняемого файла";
                    saveDialog.FileName = "decrypted_" + Path.GetFileName(FilePath);
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                        outputFilePath = saveDialog.FileName;
                    else
                        return;
                }

                progressBar.Value = 0;
                cryptoMachine.ProgressNotify += IncProgress;
                cryptoMachine.RsaNotify += ShowDecryptMessage;
                WriteActionLog($"Началась расшифровка файла ", Path.GetFileName(FilePath));
                cryptoMachine.DecryptAsync(FilePath, outputFilePath);
            }           
        }

        private void panelManagerButton_Click(object sender, EventArgs e)
        {
            if(keyPanel.Visible == false)
                keyPanel.Visible = true;
            else
                keyPanel.Visible = false;
        }

        private void WriteActionLog(string message, bool isError)
        {
            logBox.SelectionStart = logBox.Text.Length;
            logBox.SelectionBackColor = logBox.BackColor;
            logBox.AppendText("\n" + DateTime.Now.ToString()+": ");
            if(isError)
                logBox.SelectionBackColor = Color.LightPink;
            logBox.AppendText(message);
            logBox.ScrollToCaret();
        }
        private void WriteActionLog(string message, string filename)
        {
            logBox.SelectionStart = logBox.Text.Length;
            logBox.SelectionBackColor = logBox.BackColor;
            var tempstr = $"\n{DateTime.Now}: {message}";
            logBox.AppendText(tempstr);
            logBox.SelectionBackColor = Color.PaleTurquoise;
            logBox.AppendText(filename);
            logBox.ScrollToCaret();
        }
    }
}
