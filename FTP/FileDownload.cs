using FluentFTP;
using FTP_console.Index;
using System.Diagnostics;
using System.Windows.Forms;

namespace FTP_console.FTP
{
    internal class FileDownload
    {
        public Stopwatch download_file(FtpClient client)
        {
            string path = "";
            int file_index_selected;
            int file_index = 1;
            SaveFileDialog dialog = new SaveFileDialog();
            Stopwatch timer = new Stopwatch();
            FtpListItem[] items;
            IndexedFIles[] indexedFIles = new IndexedFIles[4096];
           
            Console.WriteLine("Select which in which position you want to save your files");
            dialog.Filter = "Placeholder file (*.placeholder)|*.placeholder";
            dialog.ShowDialog();

            /*
             * for some god forsaken reason the default openfolder dialog is 
             * absolute dog shit so for now i need to resort to place a temporary file as a flag
             * for the path and the delete it i hate it here
             */

            Console.WriteLine("retrieving list of files from server...");
            items = client.GetListing("\\", FtpListOption.Recursive);

            //TODO: FINISH IMPLEMETNING UPLOAD LOGIC
            /*
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



            return timer;
        }
    }
}
