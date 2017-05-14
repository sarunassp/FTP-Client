using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTP_Client
{
    class FTPClientWithTCP
    {
        #region Variables
        public bool isLoggenIn = false;
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

        TcpClient tcpClient = null;
        NetworkStream stream = null;
        StreamWriter writer = null;

        TcpClient tcpClientDataConnection = null;
        Stream streamDataConnection = null;

        Form1 form = null;
        #endregion

        public FTPClientWithTCP (Form1 form)
        {
            this.form = form;
        }

        public void LogIn (string username, string password, string host)
        {
            try
            {
                tcpClient = new TcpClient (host, ftpPort);

                stream = tcpClient.GetStream ();
                writer = new StreamWriter (tcpClient.GetStream ());
                hostResponse = GetResponseString (stream);
                statusCode = int.Parse (hostResponse.Substring (0, 1));
                if (statusCode != 2)
                {
                    form.ShowMessage ("Failed to connect to server, host didn't respond");
                    return;
                }


                writer.WriteLine ("USER " + username);
                writer.Flush ();
                hostResponse = GetResponseString (stream);
                statusCode = int.Parse (hostResponse.Substring (0, 1));
                if (!(statusCode == 2 || statusCode == 3))
                {
                    form.ShowMessage ("Failed to connect to server, wrong username");
                    return;
                }


                writer.WriteLine ("PASS " + password);
                writer.Flush ();
                hostResponse = GetResponseString (stream);
                statusCode = int.Parse (hostResponse.Substring (0, 1));
                if (statusCode != 2)
                {
                    form.ShowMessage ("Failed to connect to server, wrong password");
                    return;
                }

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

            writer.WriteLine ("TYPE I");
            writer.Flush ();
            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 1));
            if (statusCode != 2)
            {
                form.ShowMessage ("Error establishing data connection");
                return;
            }


            writer.WriteLine ("PASV");
            writer.Flush ();
            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 1));
            if (statusCode != 2)
            {
                form.ShowMessage ("Error establishing data connection");
                return;
            }

            stringList = DecodeHostAndPort (hostResponse);

            tcpClientDataConnection = new TcpClient (stringList[0], int.Parse (stringList[1]));
            streamDataConnection = tcpClientDataConnection.GetStream ();

            using (FileStream fileStream = new FileStream (localDestinationFilePath, FileMode.Create))
            {
                writer.WriteLine ("RETR " + fileToDownloadPath);
                writer.Flush ();
                hostResponse = GetResponseString (stream);
                statusCode = int.Parse (hostResponse.Substring (0, 1));
                if (statusCode != 1)
                {
                    form.ShowMessage ("Failed to open data connection");
                    return;
                }


                bytesReceived = 0;
                var buffer1 = new byte[2048];

                while (true)
                {
                    bytesReceived = streamDataConnection.Read (buffer1, 0, buffer.Length);

                    if (bytesReceived == 0)
                        break;

                    fileStream.Write (buffer1, 0, bytesReceived);
                }

                hostResponse = GetResponseString (stream);
                statusCode = int.Parse (hostResponse.Substring (0, 1));
                if (statusCode == 3 || statusCode == 4 || statusCode == 5)
                {
                    form.ShowMessage ("Failed to download file");
                    return;
                }
            }

        }

        public void UploadFile (OpenFileDialog fileDialog, string fileToUploadPath)
        {
            if (!isLoggenIn)
            {
                form.ShowMessage ("Please log in");
                return;
            }

            writer.WriteLine ("TYPE I");
            writer.Flush ();
            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 1));
            if (statusCode != 2)
            {
                form.ShowMessage ("Error establishing data connection");
                return;
            }


            writer.WriteLine ("PASV");
            writer.Flush ();
            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 1));
            if (statusCode != 2)
            {
                form.ShowMessage ("Error establishing data connection");
                return;
            }

            stringList = DecodeHostAndPort (hostResponse);

            tcpClientDataConnection = new TcpClient (stringList[0], int.Parse (stringList[1]));
            streamDataConnection = tcpClientDataConnection.GetStream ();

            using (FileStream fileStream = File.OpenRead (fileDialog.FileName))
            {
                var fileBytesBuffer = new byte[fileStream.Length];
                fileStream.Read (fileBytesBuffer, 0, fileBytesBuffer.Length);

                writer.WriteLine ("STOR " + fileToUploadPath + "/" + fileDialog.SafeFileName);
                writer.Flush ();
                hostResponse = GetResponseString (stream);
                statusCode = int.Parse (hostResponse.Substring (0, 1));
                if (statusCode != 1)
                {
                    form.ShowMessage ("Failed to open data connection");
                    return;
                }

                streamDataConnection.Write (fileBytesBuffer, 0, fileBytesBuffer.Length);
                streamDataConnection.Flush ();
            }

            if (!stream.DataAvailable)
                return;

            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 1));
            if (statusCode == 3 || statusCode == 4 || statusCode == 5)
            {
                form.ShowMessage ("Failed to download file");
                return;
            }
        }

        public string GetWorkingDirectory ()
        {
            if (!isLoggenIn)
            {
                form.ShowMessage ("Please log in");
                return "";
            }

            writer.WriteLine ("PWD");
            writer.Flush ();
            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 1));
            if (statusCode == 4 || statusCode == 5)
            {
                form.ShowMessage ("Failed to get directory");
                return "";
            }

            return hostResponse.Split ('"')[1];
        }

        public void ChangeWorkingDirectory (string newDirectory)
        {
            if (!isLoggenIn)
            {
                form.ShowMessage ("Please log in");
                return;
            }

            writer.WriteLine ("CWD " + newDirectory);
            writer.Flush ();
            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 1));

            if (statusCode == 4 || statusCode == 5)
            {
                form.ShowMessage ("Failed to change working directory");
                return;
            }
        }

        public void ChangeToParentDirectory ()
        {
            if (!isLoggenIn)
            {
                form.ShowMessage ("Please log in");
                return;
            }

            writer.WriteLine ("CDUP");
            writer.Flush ();
            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 1));

            if (statusCode == 4 || statusCode == 5)
            {
                form.ShowMessage ("Failed to change working directory");
                return;
            }
        }

        public List<string> GetWorkingDirectoryInfo ()
        {
            var workingDirectoryInfoList = new List<string> ();

            writer.WriteLine ("TYPE I");
            writer.Flush ();
            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 1));
            if (statusCode != 2)
            {
                form.ShowMessage ("Error establishing data connection");
                return null;
            }


            writer.WriteLine ("PASV");
            writer.Flush ();
            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 1));
            if (statusCode != 2)
            {
                form.ShowMessage ("Error establishing data connection");
                return null;
            }

            stringList = DecodeHostAndPort (hostResponse);

            tcpClientDataConnection = new TcpClient (stringList[0], int.Parse (stringList[1]));
            streamDataConnection = tcpClientDataConnection.GetStream ();

            writer.WriteLine ("NLST");
            writer.Flush ();
            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 1));

            if (statusCode == 4 || statusCode == 5)
            {
                form.ShowMessage ("Failed to get working directory info");
                return null;
            }

            using (StreamReader reader = new StreamReader (streamDataConnection))
            {
                string line = reader.ReadLine ();
                while (!string.IsNullOrEmpty (line))
                {
                    workingDirectoryInfoList.Add (line);
                    line = reader.ReadLine ();
                }
            }
            
            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 1));
            if (statusCode == 4 || statusCode == 5)
            {
                form.ShowMessage ("Failed to get working directory info");
                return null;
            }
            

            return workingDirectoryInfoList;
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
        
        private string GetResponseString (NetworkStream stream)
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

        public void LogOut ()
        {
            if (!isLoggenIn)
                return;

            writer.WriteLine ("QUIT");
            writer.Flush ();
            hostResponse = GetResponseString (stream);
            statusCode = int.Parse (hostResponse.Substring (0, 1));
            if (statusCode != 2)
            {
                form.ShowMessage ("Failed to log out");
                return;
            }

            tcpClient.GetStream ().Close ();
            tcpClient.Close ();
            tcpClient = null;
            writer = null;
            stream = null;

            tcpClientDataConnection.Close ();
            tcpClientDataConnection = null;
            streamDataConnection = null;

            isLoggenIn = false;
        }

    }
}
