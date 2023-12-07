using FTP_console.Config;
using FTP_console.Debug;
using FTP_console.Menus;
using FTP_console.Misc;
using Newtonsoft.Json;
public class FTPConsole
{
    /// <summary>
    /// This method handles the first boot of the software and taking in a parameter if its started
    /// from the console
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {   
        //detect if we have any args at startup and init in the way we need
        if (args.Length == 0)
        {
            init();
        }
        else
        {
            init(args[0]);
        }
        
    }

    /// <summary>
    /// this method starts checking that required files exist and also detects if we are in debug mode
    /// or any other argument passed via the cmd
    /// </summary>
    /// <param name="arg_1"></param>
    public static void init(string arg_1)
    {
        if (arg_1 == "-d")
        {
            Console.WriteLine("you seem to have started the program witha debug flag are you sure you want to run in debug mode? [Y/N]");
            var confirmation = Console.ReadLine();
            confirmation.ToLower();
            if (confirmation.Equals("y", StringComparison.OrdinalIgnoreCase))
            {
                Debug_and_test debug = new Debug_and_test();
                debug.debug_1();
            }
        }
    }

    /// <summary>
    /// init method but without any args passed in 
    /// </summary>
    public static void init()
    {
        ColorConsole color = new ColorConsole();
        while (true)
        {
            try
            {
                Config_Json config_to_serialize = new Config_Json();
                
                //check files for settings
                if (!File.Exists(".\\Config\\Settings_Config.json"))
                {
                    if (!Directory.Exists(".\\Config"))
                    {
                        Directory.CreateDirectory(".\\Config");
                    }

                    using (StreamWriter file = File.CreateText(".\\Config\\Settings_Config.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, config_to_serialize);
                    } 
                }
                //we read after creatign or if its already tehre good enough to set the colors immediately
                Config_Json config = JsonConvert.DeserializeObject<Config_Json>(File.ReadAllText(".\\Config\\Settings_Config.json"));
                color.SetColor(config.color);

                if (config.verbose)
                {
                    Console.WriteLine("Loading Settings config files");
                }

                //check files for config
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

                    if (config.verbose)
                    {
                        Console.WriteLine("Loading FTP config files");
                    }

                    using (StreamWriter file = File.CreateText(".\\Config\\FTP_Config.json"))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Formatting = Formatting.Indented;
                        serializer.Serialize(file, ftp_config);

                        color.PrintColor("!!!IF THIS IS THE FIRST TIME YOU START UP THIS APPLICATION GO INTO THE CONFIG FOLDER AND WRITE YOUR FTP LOGIN DETAILS INTO THE JSON FILE!!! \n If you see this message even if the file was created and was modified leave an issue on the Github page!", ConsoleColor.Yellow);
                        //MessageBox.Show("!!!IF THIS IS THE FIRST TIME YOU START UP THIS APPLICATION GO INTO THE CONFIG FOLDER AND WRITE YOUR FTP LOGIN DETAILS INTO THE JSON FILE!!! \n If you see this message even if the file was created and was modified leave an issue on the Github page!", "Caution!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
                //main menu startup
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
                        downloadMenu.download_menu_UI(config.verbose);
                        break;

                    case 1:
                        UploadMenu uploadMenu = new UploadMenu();
                        uploadMenu.upload_menu_UI(config.verbose);
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
                color.PrintColor(e.Message, ConsoleColor.Red);
                //MessageBox.Show(e.Message, e.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}