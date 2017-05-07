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
        private int charsReceived = 0;
        private string hostResponse = string.Empty;
        private List<string> stringList = new List<string> ();

        TcpClient tcpClient = new TcpClient ();
        Stream stream = null;
        StreamWriter writer = null;

        TcpClient tcpClientDataConnection = new TcpClient ();
        Stream streamDataConnection = null;

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
                writer = new StreamWriter (tcpClient.GetStream ());
                hostResponse = GetResponseString (stream);
                statusCode = int.Parse (hostResponse.Substring (0, 3));
                if (statusCode != 220)
                {
                    form.ShowMessage ("Failed to connect to server, host didn't respond");
                    LogOut (writer);
                }


                writer.WriteLine ("USER " + username);
                writer.Flush ();
                hostResponse = GetResponseString (stream);
                statusCode = int.Parse (hostResponse.Substring (0, 3));
                if (!(statusCode == 230 || statusCode == 331))
                {
                    form.ShowMessage ("Failed to connect to server, wrong username");
                    LogOut (writer);
                }


                writer.WriteLine ("PASS " + password);
                writer.Flush ();
                hostResponse = GetResponseString (stream);
                statusCode = int.Parse (hostResponse.Substring (0, 3));
                if (!(statusCode == 230 || statusCode == 202))
                {
                    form.ShowMessage ("Failed to connect to server, wrong password");
                    LogOut (writer);
                }


                writer.WriteLine ("TYPE I");
                writer.Flush ();
                hostResponse = GetResponseString (stream);
                statusCode = int.Parse (hostResponse.Substring (0, 3));
                if (statusCode != 200)
                {
                    form.ShowMessage ("Error from ftp server");
                    LogOut (writer);
                }


                writer.WriteLine ("PASV");
                writer.Flush ();
                hostResponse = GetResponseString (stream);
                statusCode = int.Parse (hostResponse.Substring (0, 3));
                if (statusCode != 227)
                {
                    form.ShowMessage ("Error from ftp server");
                    LogOut (writer);
                }

                stringList = DecodeHostAndPort (hostResponse);

                tcpClientDataConnection = new TcpClient (stringList[0], int.Parse (stringList[1]));
                streamDataConnection = tcpClientDataConnection.GetStream ();
            }
            catch (Exception ex)
            {
                form.ShowMessage ("Failed to log in:" + Environment.NewLine + ex.Message);
                return;
            }

            isLoggenIn = true;
        }

        public void DownloadFile (string localDestinationFilePath, string fileToDownloadPath)
        {
            if (!isLoggenIn)
            {
                form.ShowMessage ("Please log in");
                return;
            }

            using (FileStream fileStream = new FileStream (localDestinationFilePath, FileMode.Create))
            {
                writer.WriteLine ("RETR " + fileToDownloadPath);
                writer.Flush ();

                bytesReceived = 0;
                var buffer1 = new byte[2048];

                while (true)
                {
                    bytesReceived = streamDataConnection.Read (buffer1, 0, buffer.Length);

                    if (bytesReceived == 0)
                        break;

                    fileStream.Write (buffer1, 0, bytesReceived);
                }
            }
        }

        public void UploadFile (string localFileToUploadPath, string fileToUploadPath)
        {
            if (!isLoggenIn)
            {
                form.ShowMessage ("Please log in");
                return;
            }

            using (FileStream fileStream = File.OpenRead (localFileToUploadPath))
            {
                var fileBytesBuffer = new byte[fileStream.Length];
                fileStream.Read (fileBytesBuffer, 0, fileBytesBuffer.Length);

                writer.WriteLine ("STOR " + fileToUploadPath);
                writer.Flush ();

                streamDataConnection.Write (fileBytesBuffer, 0, fileBytesBuffer.Length);
                streamDataConnection.Flush ();
            }
        }

        public string GetWorkingDirectory (Stream stream)
        {
            if (!isLoggenIn)
            {
                form.ShowMessage ("Please log in");
                return "";
            }

            writer.WriteLine ("PWD");
            writer.Flush ();

            return GetResponseString (stream);
        }

        public void ChangeWorkingDirectory (string newDirectory, StreamWriter writer, Stream stream)
        {
            if (!isLoggenIn)
            {
                form.ShowMessage ("Please log in");
                return;
            }

            writer.WriteLine ("CWD " + newDirectory);
            writer.Flush ();
            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 3));

            if (!(statusCode != 501 || statusCode != 550))
            {
                form.ShowMessage ("Failed to change working directory");
            }
        }

        private List<string> DecodeHostAndPort (string hostResponse)
        {
            var intermediateResult = hostResponse.Split (' ').LastOrDefault ().Replace ("(", "").Replace (")", "").Split (',');
            var list = new List<string> ();

            string newHost = intermediateResult[0] + "." + intermediateResult[1] + "." + intermediateResult[2] + "." + intermediateResult[3];
            int newPort = (int.Parse (intermediateResult[4]) * 256) + int.Parse (intermediateResult[5]);

            list.Add (newHost);
            list.Add (newPort.ToString ());

            return list;
        }

        private string GetResponseString (Stream stream)
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

        private void LogOut (StreamWriter writer)
        {
            writer.WriteLine ("QUIT");
            writer.Flush ();

            isLoggenIn = false;
        }


    }
}
