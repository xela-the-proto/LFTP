using FluentFTP;
using System.Diagnostics;
using System.Windows.Forms;

namespace FTP_console.FTP
{
    public class FileUpload
    {
        public Stopwatch upload_file(OpenFileDialog openFileDialog, FtpClient client, bool overwrite, bool verify)
        {
            Stopwatch upload_time = new Stopwatch();
            upload_time.Start();

            Action<FtpProgress> progress = delegate (FtpProgress p)
            {
                if (p.Progress == 1)
                {
                    upload_time.Stop();
                }
                else
                {
                    Console.WriteLine("\r{0}%", p.Progress);
                }
            };

            // upload a file with progress tracking
            client.UploadFile(openFileDialog.FileName, "/" + openFileDialog.SafeFileName, FtpRemoteExists.Overwrite, true, FtpVerify.None, progress);

            return upload_time;
        }
    }
}