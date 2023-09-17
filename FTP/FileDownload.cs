using FluentFTP;
using FTP_console.Misc;
using System.Diagnostics;
using System.Windows.Forms;

namespace FTP_console.FTP
{
    internal class FileDownload
    {
        //TODO: MAKE A BETTER DOWNLAOD MENU
        public long file_size { get; set; }

        public Stopwatch download_file(FtpClient client)
        {
            Stopwatch timer = new Stopwatch();
            SaveFileDialog dialog = new SaveFileDialog();
            PathBuilder builder = new PathBuilder();
            string path = "";

            try
            {
                Action<FtpProgress> progress = delegate (FtpProgress p)
                {
                    string formatted_percentage;
                    if (p.Progress == 1)
                    {
                        timer.Stop();
                    }
                    else
                    {
                        formatted_percentage = string.Format("{0:N2}", p.Progress);
                        Console.WriteLine(formatted_percentage + "%");
                    }
                };

                Console.WriteLine("retrieving list of files from server...");

                string[] item = client.GetNameListing("\\");

                for (int i = 0; i < item.Length; i++)
                {
                    Console.WriteLine(item[i]);
                }

                Console.WriteLine("To download a file type \"download\" [insert the name of file / folder]");
                Console.WriteLine("Or type cd [insert folder name here] to navigate down a folder and cd .. to go to the top root folder");


                path = builder.build_path(client);

                Console.WriteLine(String.Format("Downloading..."));
                dialog.Filter = "Any file (*.*) | *.*";
                dialog.FileName = path.Split("\\").Last();
                dialog.ShowDialog();
                timer.Start();

                client.DownloadFile(dialog.FileName, path.TrimStart(), FtpLocalExists.Overwrite, FtpVerify.None, progress);

                timer.Stop();

                file_size = client.GetFileSize(path.TrimStart());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.TargetSite.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return timer;
        }
    }
}