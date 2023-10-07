using FTP_console.Config;
using FTP_console.Menus;
using Newtonsoft.Json;
using System.IO;
public class FTPConsole
{
    public static void Main(string[] args)
    {   
        if (args.Length == 0)
        {
            var t = new Thread(() => init());
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
        else
        {
            var t = new Thread(() => init(args[0]));
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }
        
    }

    public static void init(string arg_1)
    {
        if (arg_1 == "-d")
        {
            MessageBox.Show("haha debug go brrrrrrrrrrrr");
        }
        while (true)
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
                    if (!Directory.Exists(".\\Config"))
                    {
                        Directory.CreateDirectory(".\\Config");
                    }

                    if (settings_config.verbose)
                    {
                        Console.WriteLine("Loading Settings config files");
                    }

                    using (StreamWriter file = File.CreateText(".\\Config\\Settings_Config.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, settings_config);
                    }
                }

                if (!File.Exists(".\\Config\\FTP_Config.json"))
                {
                    if (!Directory.Exists(".\\Config"))
                    {
                        Directory.CreateDirectory(".\\Config");
                    }

                    FTP_Json ftp_config = new FTP_Json
                    {
                        host = "localhost",
                        logon_type = null,
                        password = "",
                        port = 21,
                        username = "anonymous"
                    };

                    if (settings_config.verbose)
                    {
                        Console.WriteLine("Loading FTP config files");
                    }

                    using (StreamWriter file = File.CreateText(".\\Config\\FTP_Config.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, ftp_config);
                        MessageBox.Show("!!!IF THIS IS THE FIRST TIME YOU START UP THIS APPLICATION GO INTO THE CONFIG FOLDER AND WRITE YOUR FTP LOGIN DETAILS INTO THE JSON FILE!!! \n If you see this message even if the file was created and was modified leave an issue on the Github page!", "Caution!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                int op = 0;
                string line;
                Console.WriteLine("Select which operation you would like to use");
                Console.WriteLine("[0] Quit program");
                Console.WriteLine("[1] Upload");
                Console.WriteLine("[2] Download");
                line = Console.ReadLine();
                if (line == "")
                {
                    op = Int32.MaxValue;
                }
                else op = Convert.ToInt32(line);
                switch (op)
                {
                    case 2:
                        DownloadMenu downloadMenu = new DownloadMenu();
                        downloadMenu.download_menu_UI(settings_config.verbose);
                        break;

                    case 1:
                        UploadMenu uploadMenu = new UploadMenu();
                        uploadMenu.upload_menu_UI(settings_config.verbose);
                        break;

                    case 0:
                        Console.WriteLine("Quitting...");
                        return;
                        break;

                    default:
                        break;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public static void init()
    {
        while (true)
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
                    if (!Directory.Exists(".\\Config"))
                    {
                        Directory.CreateDirectory(".\\Config");
                    }

                    if (settings_config.verbose)
                    {
                        Console.WriteLine("Loading Settings config files");
                    }

                    using (StreamWriter file = File.CreateText(".\\Config\\Settings_Config.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, settings_config);
                    }
                }

                if (!File.Exists(".\\Config\\FTP_Config.json"))
                {
                    if (!Directory.Exists(".\\Config"))
                    {
                        Directory.CreateDirectory(".\\Config");
                    }

                    FTP_Json ftp_config = new FTP_Json
                    {
                        host = "localhost",
                        logon_type = null,
                        password = "",
                        port = 21,
                        username = "anonymous"
                    };

                    if (settings_config.verbose)
                    {
                        Console.WriteLine("Loading FTP config files");
                    }

                    using (StreamWriter file = File.CreateText(".\\Config\\FTP_Config.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, ftp_config);
                        MessageBox.Show("!!!IF THIS IS THE FIRST TIME YOU START UP THIS APPLICATION GO INTO THE CONFIG FOLDER AND WRITE YOUR FTP LOGIN DETAILS INTO THE JSON FILE!!! \n If you see this message even if the file was created and was modified leave an issue on the Github page!", "Caution!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                int op = 0;
                string line;
                Console.WriteLine("Select which operation you would like to use");
                Console.WriteLine("[0] Quit program");
                Console.WriteLine("[1] Upload");
                Console.WriteLine("[2] Download");
                line = Console.ReadLine();
                if (line == "")
                {
                    op = Int32.MaxValue;
                }
                else op = Convert.ToInt32(line);
                switch (op)
                {
                    case 2:
                        DownloadMenu downloadMenu = new DownloadMenu();
                        downloadMenu.download_menu_UI(settings_config.verbose);
                        break;

                    case 1:
                        UploadMenu uploadMenu = new UploadMenu();
                        uploadMenu.upload_menu_UI(settings_config.verbose);
                        break;

                    case 0:
                        Console.WriteLine("Quitting...");
                        return;
                        break;

                    default:
                        break;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}