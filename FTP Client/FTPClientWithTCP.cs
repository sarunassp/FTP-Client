using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FTP_Client
{
    class FTPClientWithTCP
    {
        #region Variables
        private bool isLoggenIn = false;
        private string username = string.Empty;//"demo-user"
        private string password = string.Empty;//"demo-user"
        private string host = string.Empty;//"demo.wftpserver.com"

        private int ftpPort = 21;
        private int statusCode = 0;
        private static int bufferLength = 512;
        private byte[] buffer = new byte[bufferLength];
        private string result = string.Empty;
        private int bytesReceived = 0;
        private string hostResponse = string.Empty;

        TcpClient tcpClient = new TcpClient ();
        NetworkStream stream = null;
        StreamReader reader = null;
        StreamWriter writer = null;

        Form1 form = null;
        #endregion

        public FTPClientWithTCP (string username, string password, string host, Form1 form)
        {
            this.username = username;
            this.password = password;
            this.host = host;
            this.form = form;
        }

        public void LogIn ()
        {
            try
            {
                tcpClient.Connect (host, ftpPort);

                stream = tcpClient.GetStream ();
                reader = new StreamReader (tcpClient.GetStream ());
                writer = new StreamWriter (tcpClient.GetStream ());
                hostResponse = GetResponseString ();
                statusCode = int.Parse (hostResponse.Substring (0, 3));
                if (statusCode != 220)
                {
                    form.ShowMessage ("Failed to connect to server, host didn't respond");
                    LogOut ();
                }


                writer.WriteLine ("USER " + username);
                writer.Flush ();
                hostResponse = GetResponseString ();
                statusCode = int.Parse (hostResponse.Substring (0, 3));
                if (!(statusCode != 230 || statusCode != 331))
                {
                    form.ShowMessage ("Failed to connect to server, wrong username");
                    LogOut ();
                }


                writer.WriteLine ("PASS " + password);
                writer.Flush ();
                hostResponse = GetResponseString ();
                statusCode = int.Parse (hostResponse.Substring (0, 3));
                if (!(statusCode != 230 || statusCode != 202))
                {
                    form.ShowMessage ("Failed to connect to server, wrong password");
                    LogOut ();
                }

            }
            catch (Exception ex)
            {
                form.ShowMessage ("Failed to log in:" + Environment.NewLine + ex.Message);
                return;
            }

            isLoggenIn = true;
        }

        public string GetWorkingDirectory ()
        {
            writer.WriteLine ("PWD");

            return GetResponseString ();
        }

        public void ChangeWorkingDirectory (string newDirectory)
        {
            writer.WriteLine ("CWD " + newDirectory);
            hostResponse = GetResponseString ();
            statusCode = int.Parse (hostResponse.Substring (0, 3));

            if (!(statusCode != 501 || statusCode != 550))
            {
                form.ShowMessage ("Failed to change working directory");
            }
        }

        private string GetResponseString ()
        {
            hostResponse = string.Empty;
            Array.Clear (buffer, 0, buffer.Length);
            while (true)
            {
                bytesReceived = stream.Read (buffer, 0, buffer.Length);
                this.hostResponse += System.Text.Encoding.Default.GetString (buffer, 0, bytesReceived);

                if (bytesReceived < buffer.Length)
                    break;
            }

            return hostResponse;
        }

        private void LogOut ()
        {
            writer.WriteLine ("QUIT");
            writer.Flush ();

            isLoggenIn = false;
        }


    }
}
