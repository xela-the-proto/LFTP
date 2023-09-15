using FluentFTP;
using System.Diagnostics;
using System.Windows.Forms;

namespace FTP_console.FTP
{
    internal class FileDownload
    {
        //TODO: MAKE A BETTER DOWNLAOD MENU
        public Stopwatch download_file(FtpClient client)
        {
            string path = "";
            Stopwatch timer = new Stopwatch();
            SaveFileDialog dialog = new SaveFileDialog();
            string[] item = null;
            bool browsing = true;
            string file_or_folder_name = "";

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
                    //Console.WriteLine("\r{0}%", p.Progress);
                }
            };

            Console.WriteLine("retrieving list of files from server...");

            //item = client.GetListing("/", FtpListOption.Recursive);

            item = client.GetNameListing(".");

            for (int i = 0; i < item.Length; i++)
            {
                Console.WriteLine(item[i]);
            }

            Console.WriteLine("To downlaod a file type download [insert the name of file / folder]");
            Console.WriteLine("Or type cd [insert folder name here] to navigate down a folder and cd .. to go up");

            
            while (browsing)
            {
                path = Console.ReadLine();
                if (path.StartsWith("cd"))
                {
                    item = client.GetNameListing(path.Substring(3));
                }else if(path == "cd ..")
                {
                    item = client.GetNameListing(path.Substring(path.IndexOf('/') + 1));
                }
                else if (path.StartsWith("download"))
                {
                    file_or_folder_name = path.Substring(8);
                    browsing = false;

                }

                for (int i = 0; i < item.Length; i++)
                {
                    Console.WriteLine(item[i]);
                }
                
            }

            Console.WriteLine(String.Format("Downloading {0}", path));
            dialog.Filter = "Any file (*.*) | *.*";
            dialog.FileName = file_or_folder_name;
            dialog.ShowDialog();
            timer.Start();


            //client.DownloadFile(Path.GetFullPath(dialog.FileName), client.GetWorkingDirectory , FtpLocalExists.Overwrite, //FtpVerify.OnlyChecksum, progress);   


            return timer;
        }
    }
}


/*
 * OLD CODE
            for (int i = 0; i < items.Length; i++)
            {
                indexedFIles[i].index = i;
                indexedFIles[i].item = items[i];
            }

            for (int i = 0; i < indexedFIles.Length; i++)
            {
                Console.WriteLine(indexedFIles[i].toString());   
            }
            Console.WriteLine("Insert the number shown near to the entry to select it");
            file_index_selected = Convert.ToInt32(Console.ReadLine());
            */

