using FTP_console.Misc;

namespace FTP_console.Debug
{
    internal class Debug_and_test 
    {
        /// <summary>
        /// debug feature 1
        /// currently debugging: naviagtion system included in library
        /// </summary>
        public void debug_1()
        {

            PopUp popUp = new PopUp();

            popUp.Popup(Gtk.DialogFlags.Modal, Gtk.MessageType.Error, Gtk.ButtonsType.OkCancel, "test");
            /*
            Application.Init();
            

            string result = null;
            Gtk.FileChooserDialog saveDialog = new Gtk.FileChooserDialog("Save as", null, Gtk.FileChooserAction.Open, "Cancel", Gtk.ResponseType.Cancel, "Save", Gtk.ResponseType.Accept);

            if (saveDialog.Run() == (int)Gtk.ResponseType.Accept)
            {
                result = saveDialog.Filename;
            }

            saveDialog.Destroy();

            Console.Write(result);
            */


        }


    }

   
}
