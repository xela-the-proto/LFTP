﻿using FluentFTP;
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

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
                }
            };

            // upload a file with progress tracking
            client.UploadFile(openFileDialog.FileName, path + "/" + openFileDialog.SafeFileName, FtpRemoteExists.Overwrite, true, FtpVerify.None, progress);

            return upload_time;
        }
    }
}