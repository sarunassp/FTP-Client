namespace FTP_Client
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxMainDirectory = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonChangeDirectory = new System.Windows.Forms.Button();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.textBoxUsername = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxHost = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonParentDirectory = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(434, 388);
            this.listBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-3, 402);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Current directory";
            // 
            // textBoxMainDirectory
            // 
            this.textBoxMainDirectory.Enabled = false;
            this.textBoxMainDirectory.Location = new System.Drawing.Point(0, 422);
            this.textBoxMainDirectory.Name = "textBoxMainDirectory";
            this.textBoxMainDirectory.Size = new System.Drawing.Size(434, 22);
            this.textBoxMainDirectory.TabIndex = 1;
            this.textBoxMainDirectory.Tag = "";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(440, 12);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(120, 45);
            this.buttonConnect.TabIndex = 3;
            this.buttonConnect.Text = "Connect to an FTP server";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonChangeDirectory
            // 
            this.buttonChangeDirectory.Location = new System.Drawing.Point(440, 128);
            this.buttonChangeDirectory.Name = "buttonChangeDirectory";
            this.buttonChangeDirectory.Size = new System.Drawing.Size(120, 45);
            this.buttonChangeDirectory.TabIndex = 4;
            this.buttonChangeDirectory.Text = "Change directory";
            this.buttonChangeDirectory.UseVisualStyleBackColor = true;
            this.buttonChangeDirectory.Click += new System.EventHandler(this.buttonChangeDirectory_Click);
            // 
            // buttonDownload
            // 
            this.buttonDownload.Location = new System.Drawing.Point(440, 179);
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.Size = new System.Drawing.Size(120, 45);
            this.buttonDownload.TabIndex = 5;
            this.buttonDownload.Text = "Download file";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // buttonUpload
            // 
            this.buttonUpload.Location = new System.Drawing.Point(440, 230);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(120, 45);
            this.buttonUpload.TabIndex = 6;
            this.buttonUpload.Text = "Upload file";
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // textBoxUsername
            // 
            this.textBoxUsername.Location = new System.Drawing.Point(440, 84);
            this.textBoxUsername.Name = "textBoxUsername";
            this.textBoxUsername.Size = new System.Drawing.Size(167, 22);
            this.textBoxUsername.TabIndex = 9;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(613, 84);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(167, 22);
            this.textBoxPassword.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(437, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(610, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 12;
            this.label3.Text = "Password";
            // 
            // textBoxHost
            // 
            this.textBoxHost.Location = new System.Drawing.Point(613, 35);
            this.textBoxHost.Name = "textBoxHost";
            this.textBoxHost.Size = new System.Drawing.Size(167, 22);
            this.textBoxHost.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(610, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 17);
            this.label4.TabIndex = 14;
            this.label4.Text = "Host address";
            // 
            // buttonParentDirectory
            // 
            this.buttonParentDirectory.Location = new System.Drawing.Point(566, 128);
            this.buttonParentDirectory.Name = "buttonParentDirectory";
            this.buttonParentDirectory.Size = new System.Drawing.Size(120, 45);
            this.buttonParentDirectory.TabIndex = 15;
            this.buttonParentDirectory.Text = "Parent directory";
            this.buttonParentDirectory.UseVisualStyleBackColor = true;
            this.buttonParentDirectory.Click += new System.EventHandler(this.buttonParentDirectory_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 516);
            this.Controls.Add(this.buttonParentDirectory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxHost);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUsername);
            this.Controls.Add(this.buttonUpload);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.buttonChangeDirectory);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxMainDirectory);
            this.Controls.Add(this.listBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMainDirectory;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonChangeDirectory;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.TextBox textBoxUsername;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxHost;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonParentDirectory;
    }
}

