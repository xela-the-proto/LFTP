using FluentFTP;
using FTP_console.Misc;
using System.Diagnostics;
using Microsoft.Win32;

using System.IO;

namespace FTP_console.FTP
{
    /// <summary>
    /// this class handles file upload 
    /// </summary>
    public class FileUpload
    {
        public string file_path { get; set; }

        /// <summary>
        /// uploads a file with the ftp lib with an already initialized <c>FtpClient</c>
        /// </summary>
        /// <param name="client"></param>
        /// <returns>How much it took with a <c>Stopwatch</c> method </returns>
        public Stopwatch upload_file(FtpClient client)
        {
            string path;
            PathBuilder builder = new PathBuilder();

            Console.WriteLine("retrieving list of files from server...");

            string[] item = client.GetNameListing(@"\");
            

            for (int i = 0; i < item.Length; i++)
            {
                Console.WriteLine(item[i]);
            }

            Console.WriteLine("To upload a file type upload when your in the right folder");
            Console.WriteLine("Or type cd [insert folder name here] to navigate down a folder and cd .. to go to the top root folder");

            path = builder.build_path(client);

            Console.WriteLine("select file to upload");
            FileDialog dialog = new FileDialog();
           
            Console.WriteLine("uploading...");
            file_path = dialog.OpenFileDialog();

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
                }
            };

            // upload a file with progress tracking
            client.UploadFile(file_path, path + dialog.file_name, FtpRemoteExists.Overwrite, true, FtpVerify.None, progress);

            return upload_time;
        }
    }
}