using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FTP_console.FTP;
using FTP_console.Config;

namespace FTP_console.Menus
{
    internal class UploadMenu
    {
        public void main_menu(bool verbose)
        {
            int op = 0;
            StreamReader file = File.OpenText(".\\Config\\FTP_Config.json");
            JsonSerializer serializer = new JsonSerializer();
            FTP_Json ftp_config = (FTP_Json)serializer.Deserialize(file, typeof(FTP_Json));
            FileUpload upload = new FileUpload();
            FTP_Connection connection = new FTP_Connection();

            
            Console.WriteLine("how would you like to connect?");
            Console.WriteLine("[0] automatic using FTP_config.json");
            Console.WriteLine("[1] manually");
            op = Convert.ToInt32(Console.ReadLine());

            switch (op)
            {
                case 0:
                    connection.connection_manager(verbose);
                    break;
                case 1:
                    Console.WriteLine("Input the host:");
                    string host = Console.ReadLine();
                    Console.WriteLine("Input the port:");
                    int port = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Input the username:");
                    string user = Console.ReadLine();
                    Console.WriteLine("Input the password:");
                    string password = Console.ReadLine();
                    
                    FtpClient client = new FtpClient(host,user,password,port);

                    connection.connection_manager(client,verbose);
                    break;

            }
        }
    }
}
