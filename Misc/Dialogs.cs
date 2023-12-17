using Gtk;

namespace FTP_console.Misc
{
    internal class Dialogs
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
                file_name = file.Filename.Substring(file.Filename.LastIndexOf('/') + 1);

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
                file_name = file.Filename.Substring(file.Filename.LastIndexOf('/') + 1);
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

    public class PopUp
    {

        public void Popup(DialogFlags flags, MessageType messageType, ButtonsType buttonsType, string message)
        {
            string icon_path = "./Res/667.jpg";
            string error;

            switch (messageType)
            {
                case MessageType.Info:
                    error = "Info";
                    break;
                case MessageType.Warning:
                    error = "Warning";
                    break;
                case MessageType.Question:
                    error = "Question";
                    break;
                case MessageType.Error:
                    error = "Error";
                    break;
                case MessageType.Other:
                    error = "Other";
                    break;
                default:
                    error = "Error";
                    break;
            }
            Application.Init();

            // Create a new window
            Window window = new Window(error);
            window.SetDefaultSize(300, 200);
            Gdk.Pixbuf icon = new Gdk.Pixbuf(icon_path);

            // Create a new MessageDialog
            MessageDialog dialog = new MessageDialog(
                null,
                flags,
                messageType,
                buttonsType,
                message);
            dialog.Icon = icon;
            dialog.SetPosition(WindowPosition.CenterAlways);
            // Set the title for the dialog
            dialog.Title = error;

            // Display the dialog and wait for a response
            dialog.Run();
            
            // Close the dialog
            dialog.Destroy();
        }
    }
}
