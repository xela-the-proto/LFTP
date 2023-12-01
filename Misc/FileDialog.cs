using Gtk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP_console.Misc
{
    internal class FileDialog
    {
        public string file_name { get; set; }
        public string OpenFileDialog()
        {
            Application.Init();

            FileChooserDialog file = new FileChooserDialog("Open file", null, FileChooserAction.Open);
            file.AddButton(Stock.Cancel, ResponseType.Cancel);
            file.AddButton(Stock.Ok, ResponseType.Ok);
            file.DefaultResponse = Gtk.ResponseType.Ok;
            file.SelectMultiple = false;

            Gtk.ResponseType response = (Gtk.ResponseType)file.Run();
            if (response == Gtk.ResponseType.Ok)
            {
                string file_path = file.Filename;
                //god help the soul of whoever finds this
                file_name = file.Filename.Substring(file.Filename.LastIndexOf('\\') + 1);

                file.Destroy();
                return file_path;
            }
            else
            {
                file.Destroy();
                return "";
            }
        }

        public string SaveFileDialog(string filename_ftp)
        {
            Application.Init();

            FileChooserDialog file = new FileChooserDialog("Open file", null, FileChooserAction.Save);
            file.AddButton(Stock.Cancel, ResponseType.Cancel);
            file.AddButton(Stock.SaveAs, ResponseType.Ok);
            file.DefaultResponse = Gtk.ResponseType.Ok;
            file.SelectMultiple = false;
            file.CurrentName = filename_ftp;
            Gtk.ResponseType response = (Gtk.ResponseType)file.Run();
            if (response == Gtk.ResponseType.Ok)
            {
                string file_path = file.Filename;
                //same as above
                file_name = file.Filename.Substring(file.Filename.LastIndexOf('\\') + 1);
                file.Destroy();
                return file_path;
            }
            else
            {
                file.Destroy();
                return "";
            }
        }
    }
}
