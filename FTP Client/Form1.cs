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
        FTPClient ftpClient;
        string currentDirectory;

        public Form1 ()
        {
            ftpClient = new FTPClient (this);

            InitializeComponent ();
            textBoxMainDirectory.Text = "/";
        }

        public void showMessage (string msg)
        {
            MessageBox.Show (msg);
        }

        private async void buttonConnect_Click (object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty (textBoxUsername.Text) || string.IsNullOrEmpty (textBoxPassword.Text) ||
                string.IsNullOrEmpty (textBoxHost.Text))
                return;

            this.ftpClient = new FTPClient (textBoxUsername.Text, textBoxPassword.Text, textBoxHost.Text, this);
            listBox1.DataSource = await Task.Run (() => ftpClient.GetDirectoryInfo (""));

        }

        private void buttonDownload_Click (object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty (textBoxUsername.Text) || string.IsNullOrEmpty (textBoxPassword.Text) ||
                string.IsNullOrEmpty (textBoxHost.Text))
                return;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog ();

            saveFileDialog1.InitialDirectory = "c:\\";
            if (saveFileDialog1.ShowDialog () == DialogResult.OK)
            {

                ftpClient.DownloadFile (saveFileDialog1.FileName, currentDirectory + listBox1.Text);
            }

        }

        private void buttonUpload_Click (object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty (textBoxUsername.Text) || string.IsNullOrEmpty (textBoxPassword.Text) ||
                string.IsNullOrEmpty (textBoxHost.Text))
                return;

            OpenFileDialog openFileDialog1 = new OpenFileDialog ();

            openFileDialog1.InitialDirectory = "c:\\";
            if (openFileDialog1.ShowDialog () == DialogResult.OK)
            {
                ftpClient.UploadFile (openFileDialog1.FileName, currentDirectory);
            }
        }

        private async void buttonChangeDirectory_Click (object sender, EventArgs e)
        {
            listBox1.DataSource = await Task.Run (() => ftpClient.GetDirectoryInfo (textBoxMainDirectory.Text));

            if (string.IsNullOrEmpty (listBox1.Text))
                return;
            textBoxMainDirectory.Text += listBox1.SelectedItem.ToString () + "/";
            currentDirectory = textBoxMainDirectory.Text;
         }

        private async void buttonParentDirectory_Click (object sender, EventArgs e)
        {
            if (textBoxMainDirectory.Text == "/")
                return;

            listBox1.DataSource = await Task.Run (() => ftpClient.GetDirectoryInfo (""));

            textBoxMainDirectory.Text = textBoxMainDirectory.Text.TrimEnd ('/');
            textBoxMainDirectory.Text = textBoxMainDirectory.Text.Remove (textBoxMainDirectory.Text.LastIndexOf ('/') + 1);
            currentDirectory = textBoxMainDirectory.Text;

            
        }


    }
}
