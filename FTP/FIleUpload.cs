using FluentFTP;
using System.Diagnostics;
using System.Windows.Forms;

namespace FTP_console.FTP
{
    public class FileUpload
    {
        public string file_path { get; set; }

        public Stopwatch upload_file(FtpClient client)
        {
            string confirmation;
            bool overwrite = false;
            bool verify = false;

            Console.WriteLine("select file to upload");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            long size = client.GetFileSize(openFileDialog.FileName);

            Console.WriteLine("Overwrite files with the same name? [Y/N]");
            confirmation = Console.ReadLine();
            confirmation.ToUpper();

            if (confirmation == "Y")
            {
                overwrite = true;
            }
            else overwrite = false;

            Console.WriteLine("Verify file integroty after upload? [Y/N]");
            confirmation = Console.ReadLine();
            confirmation.ToUpper();
            if (confirmation == "Y")
            {
                verify = true;
            }
            else verify = false;

            Console.WriteLine("uploading...");
            file_path = openFileDialog.FileName;

            Stopwatch upload_time = new Stopwatch();
            upload_time.Start();

            Action<FtpProgress> progress = delegate (FtpProgress p)
            {
                string formatted_percentage;
                if (p.Progress == 1)
                {
                    upload_time.Stop();
                }
                else
                {
                    formatted_percentage = string.Format("{0:N2}", p.Progress);
                    Console.WriteLine(formatted_percentage + "%");
                    //Console.WriteLine("\r{0}%", p.Progress);
                }
            };

            // upload a file with progress tracking
            client.UploadFile(openFileDialog.FileName, "/" + openFileDialog.SafeFileName, FtpRemoteExists.Overwrite, true, FtpVerify.None, progress);

            return upload_time;
        }
    }
}