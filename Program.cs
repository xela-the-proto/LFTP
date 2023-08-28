using FTP_console.Config;
using FTP_console.FTP;
using FTPconsole.FTP;
using Newtonsoft.Json;
using System.Windows.Forms;

public class FTPConsole
{
    public static void Main()
    {
        Thread thread = new Thread(init);
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        
    }

    public static void init()
    {
        try
        {

            Config_Json settings_config = new Config_Json
            {
                verbose = true
            };
            //first init
            if (!File.Exists(".\\Config\\Settings_Config.json"))
            {
                if (settings_config.verbose)
                {
                    Console.WriteLine("Loading Settings config files");
                }

                if (!Directory.Exists(".\\Config"))
                {
                    Directory.CreateDirectory(".\\Config");
                }

                using (StreamWriter file = File.CreateText(".\\Config\\Settings_Config.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, settings_config);
                }

            }
            else if (!File.Exists(".\\Config\\FTP_Config.json"))
            {
                FTP_Json ftp_config = new FTP_Json
                {
                    host = "localhost",
                    logon_type = null,
                    password = "",
                    port = 21,
                    username = "anonymus"
                };

                if (settings_config.verbose)
                {
                    Console.WriteLine("Loading FTP config files");
                }

                if (!Directory.Exists(".\\Config"))
                {
                    Directory.CreateDirectory(".\\Config");
                }

                using (StreamWriter file = File.CreateText(".\\Config\\FTP_Config.json"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(file, ftp_config);
                }
            }

            FTP ftp_manager = new FTP();
            ftp_manager.connection_manager();
        }
        catch (Exception e)
        {
            System.Windows.Forms.MessageBox.Show(e.Message, e.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}