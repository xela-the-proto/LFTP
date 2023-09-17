﻿using FluentFTP;

namespace FTP_console.Misc
{
    internal class PathBuilder
    {
        private string command;
        private string[] item;
        private string path;
        private string path_check;
        private bool browsing = true;

        public string build_path(FtpClient client)
        {
            while (browsing)
            {
                try
                {
                    //TODO: FIND A BETTER WAY TO BROWSE
                    command = Console.ReadLine();

                    if (!command.StartsWith("cd") && !command.StartsWith("cd ..") && !command.StartsWith("download") && 
                        command != "upload")
                    {
                        throw new FormatException("Bad command! only supported commands are \"cd\", \"cd ..\", \"download\"");
                    }

                    if (command.TrimStart().StartsWith("cd", StringComparison.OrdinalIgnoreCase))
                    {
                        item = client.GetNameListing(command.Substring(3));
                        path_check = path + command.Substring(3) + @"\";
                        if (!client.DirectoryExists(path_check))
                        {
                            throw new MissingFieldException("Directory doesn't exist!");
                        }
                        path = path + command.Substring(3) + @"\";
                    }

                    if (command.TrimStart().StartsWith("cd ..", StringComparison.OrdinalIgnoreCase))
                    {
                        item = client.GetNameListing(".");
                        path = @"\";
                    }

                    if (command.StartsWith("download"))
                    {
                        path = path + command.Substring(9);
                        browsing = false;
                    }
                    if(command == "upload")
                    {
                        browsing = false;
                    }

                    for (int i = 0; i < item.Length; i++)
                    {
                        if (!browsing)
                        {
                            break;
                        }
                        Console.WriteLine(item[i]);
                    }
                }
                catch (MissingFieldException e)
                {
                    Console.WriteLine($"{e.Message}", e);
                }catch (FormatException e)
                {
                    Console.WriteLine($"{e.Message}", e);
                }
            }
            return path;
        }
    }
}