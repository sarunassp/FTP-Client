using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTP_Client
{
    public partial class Form1 : Form
    {
        //FTPClient ftpClient;
        FTPClientWithTCP ftpClient;
        string listBoxText;

        public Form1 ()
        {
            ftpClient = new FTPClientWithTCP (this);

            InitializeComponent ();
            textBoxUsername.Text = "demo-user";
            textBoxPassword.Text = "demo-user";
            textBoxHost.Text = "demo.wftpserver.com";
        }

        public void ShowMessage (string msg)
        {
            MessageBox.Show (msg);
        }

        private async void buttonConnect_Click (object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty (textBoxUsername.Text) || string.IsNullOrEmpty (textBoxPassword.Text) ||
                string.IsNullOrEmpty (textBoxHost.Text))
                return;

            if (ftpClient.isLoggenIn)
            {
                ShowMessage ("Please log out first");
                return;
            }


            await Task.Run (() => ftpClient.LogIn(textBoxUsername.Text, textBoxPassword.Text, textBoxHost.Text));
            listBox1.DataSource = await Task.Run (() => ftpClient.GetWorkingDirectoryInfo ());
            textBoxMainDirectory.Text = await Task.Run (() => ftpClient.GetWorkingDirectory ());

        }

        private void buttonDownload_Click (object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty (textBoxUsername.Text) || string.IsNullOrEmpty (textBoxPassword.Text) ||
                string.IsNullOrEmpty (textBoxHost.Text))
                return;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog ();

            saveFileDialog1.InitialDirectory = @"C:\Users\Lenovo\Desktop\tinklai\FtpTest";
            if (saveFileDialog1.ShowDialog () == DialogResult.OK)
            {
                ftpClient.DownloadFile (saveFileDialog1.FileName, textBoxMainDirectory.Text + "/" + listBox1.Text);
            }

        }

        private void buttonUpload_Click (object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty (textBoxUsername.Text) || string.IsNullOrEmpty (textBoxPassword.Text) ||
                string.IsNullOrEmpty (textBoxHost.Text))
                return;

            OpenFileDialog openFileDialog1 = new OpenFileDialog ();

            openFileDialog1.InitialDirectory = @"C:\Users\Lenovo\Desktop\tinklai\FtpTest";
            if (openFileDialog1.ShowDialog () == DialogResult.OK)
            {
                ftpClient.UploadFile (openFileDialog1, textBoxMainDirectory.Text);
            }
        }

        private void buttonChangeDirectory_Click (object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty (listBox1.Text) || !ftpClient.isLoggenIn)
                return;
            listBoxText = listBox1.Text;
            ftpClient.ChangeWorkingDirectory (textBoxMainDirectory.Text + listBoxText);
            textBoxMainDirectory.Text = ftpClient.GetWorkingDirectory ();
            listBox1.DataSource = ftpClient.GetWorkingDirectoryInfo ();
        }

        private async void buttonParentDirectory_Click (object sender, EventArgs e)
        {
            if (textBoxMainDirectory.Text == "/" || !ftpClient.isLoggenIn)
                return;
            await Task.Run (() => ftpClient.ChangeToParentDirectory ());
            textBoxMainDirectory.Text = await Task.Run (() => ftpClient.GetWorkingDirectory ());
            listBox1.DataSource = await Task.Run (() => ftpClient.GetWorkingDirectoryInfo());
        }

        private void buttonLogOut_Click (object sender, EventArgs e)
        {
            ftpClient.LogOut ();
            listBox1.DataSource = null;
            textBoxMainDirectory.Text = "";
        }
    }
}
