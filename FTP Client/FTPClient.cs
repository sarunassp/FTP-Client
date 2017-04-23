using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTP_Client
{
    class FTPClient
    {
        string username;
        string password;
        string hostname;
        NetworkCredential networkCredentials = null;
        Form1 form;
        byte[] buffer;

        public FTPClient (Form1 form)
        {
            this.form = form;
            this.networkCredentials = new NetworkCredential (username, password);
        }

        public FTPClient (string username, string password, string hostname, Form1 form)
        {
            this.username = username;
            this.password = password;
            this.hostname = hostname;
            this.form = form;

            this.networkCredentials = new NetworkCredential (username, password);
        }


        public void UploadFile (string uploadFilePath, string uploadPath)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create (hostname + uploadPath + "/" + Path.GetFileName (uploadFilePath));

                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = networkCredentials;

                FileStream stream = File.OpenRead (uploadFilePath);
                buffer = new byte[stream.Length];

                stream.Read (buffer, 0, buffer.Length);
                stream.Close ();

                Stream reqStream = request.GetRequestStream ();
                reqStream.Write (buffer, 0, buffer.Length);
                reqStream.Close ();
            }
            catch (Exception ex)
            {
                form.showMessage ("An error has occured" + Environment.NewLine +
                                  "ERROR" + ex.Message);
            }
        }


        public void DownloadFile (string localDestinationFilePath, string fileToDownloadPath)
        {

            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create (hostname + "/" + fileToDownloadPath);

                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = networkCredentials;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse ();
                Stream streamReader = response.GetResponseStream ();
                FileStream fileStream = new FileStream (localDestinationFilePath, FileMode.Create);

                int bytesRead = 0;
                buffer = new byte[2048];

                while (true)
                {
                    bytesRead = streamReader.Read (buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                        break;

                    fileStream.Write (buffer, 0, bytesRead);
                }

                fileStream.Close ();
                streamReader.Close ();
            }
            catch (Exception ex)
            {
                form.showMessage ("An error has occured" + Environment.NewLine +
                                  "ERROR" + ex.Message);
            }
        }


        public List<string> GetDirectoryInfo (string currentDirectory)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create (hostname + "/" + currentDirectory);
                request.Credentials = networkCredentials;
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse ();
                StreamReader streamReader = new StreamReader (response.GetResponseStream ());

                var list = new List<string> ();

                string line = streamReader.ReadLine ();
                while (!string.IsNullOrEmpty (line))
                {
                    list.Add (line);
                    line = streamReader.ReadLine ();
                }

                streamReader.Close ();

                return list;
            }
            catch (Exception ex)
            {
                form.showMessage ("An error has occured" + Environment.NewLine +
                                  "ERROR" + ex.Message);
                return null;
            }
        }


        public void DeleteFile (string fileToDeletePath, string fileToDeleteName)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create (hostname + fileToDeletePath + "/" + fileToDeleteName);

                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = networkCredentials;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse ();

                response.Close ();
            }
            catch (Exception ex)
            {
                form.showMessage ("An error has occured" + Environment.NewLine +
                                  "ERROR" + ex.Message);
            }
        }


    }
}
