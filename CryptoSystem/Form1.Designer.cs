namespace CryptoSystem
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.EncryptButton = new System.Windows.Forms.Button();
            this.ChooseFileButton = new System.Windows.Forms.Button();
            this.CreateKeysButton = new System.Windows.Forms.Button();
            this.keySizeTrackBar = new System.Windows.Forms.TrackBar();
            this.keySizeBox = new System.Windows.Forms.TextBox();
            this.DecryptButton = new System.Windows.Forms.Button();
            this.keySizeCaption = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.keyPanel = new System.Windows.Forms.Panel();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.panelManagerButton = new System.Windows.Forms.Button();
            this.filePathBox = new System.Windows.Forms.TextBox();
            this.logBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.keySizeTrackBar)).BeginInit();
            this.keyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // EncryptButton
            // 
            this.EncryptButton.Location = new System.Drawing.Point(405, 189);
            this.EncryptButton.Name = "EncryptButton";
            this.EncryptButton.Size = new System.Drawing.Size(95, 36);
            this.EncryptButton.TabIndex = 0;
            this.EncryptButton.Text = "Зашифровать файл";
            this.EncryptButton.UseVisualStyleBackColor = true;
            this.EncryptButton.Click += new System.EventHandler(this.EncryptButton_Click);
            // 
            // ChooseFileButton
            // 
            this.ChooseFileButton.Location = new System.Drawing.Point(506, 160);
            this.ChooseFileButton.Name = "ChooseFileButton";
            this.ChooseFileButton.Size = new System.Drawing.Size(96, 23);
            this.ChooseFileButton.TabIndex = 1;
            this.ChooseFileButton.Text = "Выбрать файл";
            this.ChooseFileButton.UseVisualStyleBackColor = true;
            this.ChooseFileButton.Click += new System.EventHandler(this.ChooseFileButton_Click);
            // 
            // CreateKeysButton
            // 
            this.CreateKeysButton.Location = new System.Drawing.Point(12, 12);
            this.CreateKeysButton.Name = "CreateKeysButton";
            this.CreateKeysButton.Size = new System.Drawing.Size(104, 41);
            this.CreateKeysButton.TabIndex = 4;
            this.CreateKeysButton.Text = "Сгенерировать RSA ключи";
            this.CreateKeysButton.UseVisualStyleBackColor = true;
            this.CreateKeysButton.Click += new System.EventHandler(this.CreateKeysButton_Click);
            // 
            // keySizeTrackBar
            // 
            this.keySizeTrackBar.LargeChange = 1;
            this.keySizeTrackBar.Location = new System.Drawing.Point(12, 85);
            this.keySizeTrackBar.Maximum = 7;
            this.keySizeTrackBar.Minimum = 1;
            this.keySizeTrackBar.Name = "keySizeTrackBar";
            this.keySizeTrackBar.Size = new System.Drawing.Size(104, 45);
            this.keySizeTrackBar.TabIndex = 5;
            this.keySizeTrackBar.Value = 1;
            this.keySizeTrackBar.Scroll += new System.EventHandler(this.keySizeTrackBar_Scroll);
            // 
            // keySizeBox
            // 
            this.keySizeBox.Location = new System.Drawing.Point(67, 59);
            this.keySizeBox.Name = "keySizeBox";
            this.keySizeBox.ReadOnly = true;
            this.keySizeBox.Size = new System.Drawing.Size(40, 20);
            this.keySizeBox.TabIndex = 6;
            this.keySizeBox.Text = "2";
            // 
            // DecryptButton
            // 
            this.DecryptButton.Location = new System.Drawing.Point(506, 189);
            this.DecryptButton.Name = "DecryptButton";
            this.DecryptButton.Size = new System.Drawing.Size(95, 36);
            this.DecryptButton.TabIndex = 13;
            this.DecryptButton.Text = "Расшифровать файл";
            this.DecryptButton.UseVisualStyleBackColor = true;
            this.DecryptButton.Click += new System.EventHandler(this.DecryptButton_Click);
            // 
            // keySizeCaption
            // 
            this.keySizeCaption.AutoSize = true;
            this.keySizeCaption.Location = new System.Drawing.Point(19, 62);
            this.keySizeCaption.Name = "keySizeCaption";
            this.keySizeCaption.Size = new System.Drawing.Size(46, 13);
            this.keySizeCaption.TabIndex = 14;
            this.keySizeCaption.Text = "Размер";
            // 
            // progressBar
            // 
            this.progressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.progressBar.Location = new System.Drawing.Point(12, 132);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(590, 22);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 15;
            // 
            // keyPanel
            // 
            this.keyPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.keyPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.keyPanel.Controls.Add(this.CreateKeysButton);
            this.keyPanel.Controls.Add(this.keySizeTrackBar);
            this.keyPanel.Controls.Add(this.keySizeBox);
            this.keyPanel.Controls.Add(this.keySizeCaption);
            this.keyPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.keyPanel.Location = new System.Drawing.Point(608, 33);
            this.keyPanel.Name = "keyPanel";
            this.keyPanel.Size = new System.Drawing.Size(132, 121);
            this.keyPanel.TabIndex = 16;
            this.keyPanel.Visible = false;
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 21);
            // 
            // panelManagerButton
            // 
            this.panelManagerButton.Location = new System.Drawing.Point(608, 12);
            this.panelManagerButton.Name = "panelManagerButton";
            this.panelManagerButton.Size = new System.Drawing.Size(132, 23);
            this.panelManagerButton.TabIndex = 17;
            this.panelManagerButton.Text = "Создать ключи";
            this.panelManagerButton.UseVisualStyleBackColor = true;
            this.panelManagerButton.Click += new System.EventHandler(this.panelManagerButton_Click);
            // 
            // filePathBox
            // 
            this.filePathBox.Location = new System.Drawing.Point(12, 162);
            this.filePathBox.Name = "filePathBox";
            this.filePathBox.ReadOnly = true;
            this.filePathBox.Size = new System.Drawing.Size(488, 20);
            this.filePathBox.TabIndex = 18;
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(12, 12);
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.Size = new System.Drawing.Size(590, 114);
            this.logBox.TabIndex = 19;
            this.logBox.Text = "\n\n\n\n\n\n";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(751, 243);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.filePathBox);
            this.Controls.Add(this.panelManagerButton);
            this.Controls.Add(this.keyPanel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.DecryptButton);
            this.Controls.Add(this.ChooseFileButton);
            this.Controls.Add(this.EncryptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RSA Master";
            ((System.ComponentModel.ISupportInitialize)(this.keySizeTrackBar)).EndInit();
            this.keyPanel.ResumeLayout(false);
            this.keyPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button EncryptButton;
        private System.Windows.Forms.Button ChooseFileButton;
        private System.Windows.Forms.Button CreateKeysButton;
        private System.Windows.Forms.TrackBar keySizeTrackBar;
        private System.Windows.Forms.TextBox keySizeBox;
        private System.Windows.Forms.Button DecryptButton;
        private System.Windows.Forms.Label keySizeCaption;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel keyPanel;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.Button panelManagerButton;
        private System.Windows.Forms.TextBox filePathBox;
        private System.Windows.Forms.RichTextBox logBox;
    }
}

