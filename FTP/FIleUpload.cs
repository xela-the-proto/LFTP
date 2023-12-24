using FluentFTP;
using FTP_console.Misc;
using System.Diagnostics;

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
        public Stopwatch upload_file(FtpClient client, bool multiple_downloads)
        {
            string path;
            PathBuilder builder = new PathBuilder();
            PopUp popUp = new PopUp();
            List<string> files = new List<string>();
            try
            {
                

                Console.WriteLine("retrieving list of files from server...");

                string[] item = client.GetNameListing(@"/");


                for (int i = 0; i < item.Length; i++)
                {
                    Console.WriteLine(item[i]);
                }

                Console.WriteLine("To upload a file or mutliple files type upload when your in the right folder");
                Console.WriteLine("Or type cd [insert folder name here] to navigate down a folder and cd .. to go to the top root folder");

                path = builder.build_path_upload(client);

                Console.WriteLine("select file(s) to upload");
                Dialogs dialog = new Dialogs();

                if (multiple_downloads)
                {
                    while (true)
                    {
                        files.Add(dialog.OpenFileDialog());
                        Console.WriteLine("add another file? [Y/N]");
                        string conf = Console.ReadLine();
                        if (conf.Equals("N",StringComparison.CurrentCultureIgnoreCase))
                        {
                            break;
                        }
                    }
                }else
                {
                    file_path = dialog.OpenFileDialog();
                }
                

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
                        Thread.Sleep(500);
                    }
                };

                // upload a file with progress tracking
                Console.WriteLine("uploading...");
                client.UploadFiles(files, path, FtpRemoteExists.Overwrite, false, FtpVerify.None, FtpError.Throw, progress);



                client.UploadFile(file_path, path + dialog.file_name, FtpRemoteExists.Overwrite, true, FtpVerify.None, progress);

                return upload_time;
            }
            catch(DivideByZeroException e)
            {
                popUp.Popup(Gtk.DialogFlags.Modal, Gtk.MessageType.Error, Gtk.ButtonsType.Ok, e.Message);
                throw;
            }
        }
    }
}