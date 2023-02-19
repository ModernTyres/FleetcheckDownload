using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FleetcheckDownLoad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string host = "ftp://fleetchecker.moderntyres.com";
            string username = "fleetchecker";
            string password = "kUHa3$DuprUT";
            FTP ftp = new FTP(host, username, password);
            List<string> files = new List<string>();
            files = ftp.GetFiles("/FleetCheck_Upload");
            dataGridView1.DataSource = files.Select(x => new { Value = x }).ToList();
            

        }

       
    }

    public class FTP
    {
        private string _host;
        private NetworkCredential _credentials;
        private WebClient _wc;
        private string _username;
        private string _password;
        public FTP(string host,  string username, string password)
        {
            _host = host;
            _username = username;
            _password = password;
            _credentials = new NetworkCredential(_username, _password);
            _wc = new WebClient()
            {

                Credentials = _credentials
            };

            //WebClient client = new WebClient();
            //string url = "ftp://ftp.example.com/remote/path/file.zip";
            //https://www.c-sharpcorner.com/article/working-with-ftp-using-c-sharp/
            //client.Credentials = new NetworkCredential("username", "password");
            //byte[] contents = client.DownloadData(url);
        }
        private WebRequest CreateRequest(string path, string method)
        {
            var req = WebRequest.Create(_host + path);
            req.Method = method;
            req.Credentials = _credentials;
            return req;
        }

        public List<string> GetFiles(string folderpath)
        {
            try
            {
               // FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ParentFolderpath);
                var req = CreateRequest(folderpath, WebRequestMethods.Ftp.ListDirectory);
                var resp = req.GetResponse();
                var sr = new StreamReader(resp.GetResponseStream());
                var l = new List<string>();
                List<string> files = new List<string>();
                string line = sr.ReadLine();

                string s = null;
                while (!string.IsNullOrEmpty(line))
                {
                    var lineArr = line.Split('/');
                    line = lineArr[lineArr.Count() - 1];
                    files.Add(line);
                    line = sr.ReadLine();
                }

                sr.Close();

                return files;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

    }
}
//https://www.youtube.com/watch?v=iwvJ26DTK3w