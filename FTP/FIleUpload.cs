using FluentFTP;
using FTP_console.Misc;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FTP_console.FTP
{
    public class FileUpload
    {
        public string file_path { get; set; }

        public Stopwatch upload_file(FtpClient client)
        {
            string path;
            string confirmation;
            bool overwrite = false;
            bool verify = false;
            PathBuilder builder = new PathBuilder();

            Console.WriteLine("retrieving list of files from server...");

            string[] item = client.GetNameListing("\\");

            for (int i = 0; i < item.Length; i++)
            {
                Console.WriteLine(item[i]);
            }

            Console.WriteLine("To upload a file type \"upload\" when your in the right folder");
            Console.WriteLine("Or type cd [insert folder name here] to navigate down a folder and cd .. to go to the top root folder");

            path = builder.build_path(client);

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
            client.UploadFile(openFileDialog.FileName, path + "/" + openFileDialog.SafeFileName, FtpRemoteExists.Overwrite, true, FtpVerify.None, progress);

            return upload_time;
        }
    }
}