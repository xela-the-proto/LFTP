﻿using FluentFTP;
using FTP_console.FTP;

namespace FTP_console.Menus
{
    internal class DownloadMenu
    {
        private static string type_of_op = "D";
        /// <summary>
        /// Handle ui to choose how to connect
        /// </summary>
        /// <param name="verbose"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void download_menu_UI(bool verbose)
        {
            int op = 0;
            FTP_Connection connection = new FTP_Connection();

            Console.WriteLine("how would you like to connect to download your files?");
            Console.WriteLine("[0] automatic using FTP_config.json");
            Console.WriteLine("[1] manually");
            op = Convert.ToInt32(Console.ReadLine());

            switch (op)
            {
                case 0:
                    connection.connection_manager(verbose, type_of_op);
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

                    FtpClient client = new FtpClient(host, user, password, port);

                    connection.connection_manager(client, verbose, type_of_op);
                    break;
                default:
                    throw new InvalidOperationException("Unexpected value");
                    break;
            }
        }
    }
}