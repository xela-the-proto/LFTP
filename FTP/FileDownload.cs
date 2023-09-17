using FluentFTP;
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
            string command = "";
            string path = "";
            string path_check = "";
            string[] item = null;
            bool browsing = true;

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

                item = client.GetNameListing("\\");

                for (int i = 0; i < item.Length; i++)
                {
                    Console.WriteLine(item[i]);
                }

                Console.WriteLine("To download a file type download [insert the name of file / folder]");
                Console.WriteLine("Or type cd [insert folder name here] to navigate down a folder and cd .. to go to the top root folder");

                while (browsing)
                {
                    //TODO: FIND A BETTER WAY TO BROWSE
                    command = Console.ReadLine();
                    if (command.TrimStart().StartsWith("cd", StringComparison.OrdinalIgnoreCase))
                    {
                        item = client.GetNameListing(command.Substring(3));
                        path_check = path + command.Substring(3) + @"\";
                        if (!client.DirectoryExists(path_check))
                        {
                            throw new MissingFieldException("Directory doesn't exist!");
                        }
                        path = path + command.Substring(3) + @"\";
                    }
                    if (command.TrimStart().StartsWith("cd ..", StringComparison.OrdinalIgnoreCase))
                    {
                        item = client.GetNameListing(".");
                        path = @"\";
                    }
                    if (command.StartsWith("download"))
                    {
                        path = path + command.Substring(9);
                        browsing = false;
                    }

                    for (int i = 0; i < item.Length; i++)
                    {
                        if (!browsing)
                        {
                            break;
                        }
                        Console.WriteLine(item[i]);
                    }
                }

                Console.WriteLine(String.Format("Downloading {0}", command));
                dialog.Filter = "Any file (*.*) | *.*";
                dialog.FileName = path.Split("\\").Last();
                dialog.ShowDialog();
                timer.Start();

                client.DownloadFile(dialog.FileName, path.TrimStart(), FtpLocalExists.Overwrite, FtpVerify.None, progress);

                timer.Stop();

                file_size = client.GetFileSize(path.TrimStart());
            }
            catch (MissingFieldException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.TargetSite.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return timer;
        }
    }
}