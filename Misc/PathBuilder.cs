﻿using FluentFTP;
using System.Globalization;

namespace FTP_console.Misc
{
    internal class PathBuilder
    {
        private string command;
        private string[] item;
        private string path;

        private bool browsing = true;
        private bool bad_command;

        /// <summary>
        /// Handles the download remote browsing with string manipualtion
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public string build_path(FtpClient client)
        {
            path = @"\";
            while (browsing)
            {
                try
                {
                    
                    //TODO: FIND A BETTER WAY TO BROWSE
                    if (bad_command)
                    {
                        bad_command = false;
                        if (path == null)
                        {
                            path = @"\";
                        }
                        item = client.GetNameListing(path);
                        for (int i = 0; i < item.Length; i++)
                        {
                            Console.WriteLine(item[i]);
                        }
                        
                    }
                    
                    command = Console.ReadLine();

                    //check command
                    if (!command.StartsWith("cd") && !command.StartsWith("cd ..") && !command.StartsWith("download") && 
                        command != "upload")
                    {
                        throw new FormatException(@"Bad command! only supported commands are cd, cd .., download, upload");
                    }

                    //if its cd.. go up 
                    if (command.TrimStart().Equals("cd ..", StringComparison.OrdinalIgnoreCase))
                    {
                        int last_index = path.LastIndexOf(@"\", StringComparison.InvariantCulture);

                        if (last_index >= 0)
                        {
                            int second_last_index = path.LastIndexOf(@"\", last_index - 1, StringComparison.InvariantCulture);
                            if (second_last_index >= 0)
                            {
                                path = path.Substring(0, second_last_index + 1);
                            }
                        }

                        item = client.GetNameListing(path);
                    }
                    //if its cd folder build a new path to send to the server
                    else if (command.TrimStart().StartsWith("cd", StringComparison.OrdinalIgnoreCase))
                    {
                        string path_check;
                        item = client.GetNameListing(command.Substring(3));
                        path_check = path + command.Substring(3) + @"\";
                        if (!client.DirectoryExists(path_check))
                        {
                            bad_command = true;
                            throw new MissingFieldException("Directory doesn't exist!");
                        }
                        else { 
                            path = path + command.Substring(3) + @"\"; 
                        }

                    }

                    //break from the cycle and get the file to download
                    if (command.StartsWith("download"))
                    {
                        path = path + command.Substring(9);
                        browsing = false;
                    }
                    //break from the cycle and get the path were to upload
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
                        //write all the items we get from the server in a directory 
                        Console.WriteLine(item[i]);
                    }
                }
                catch (MissingFieldException e)
                {
                    Console.WriteLine($"{e.Message}", e);
                }
                catch (FormatException e)
                {
                    Console.WriteLine($"{e.Message}", e);
                }catch (ArgumentException e)
                {
                    Console.WriteLine("Bad command format!");
                }
            }
            return path;
        }
    }
}