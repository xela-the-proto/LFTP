using FluentFTP;
using FluentFTP.Exceptions;
using FTP_console.Config;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FTP_console.FTP
{
    /// <summary>
    /// this class handles creating the connections
    /// </summary>
    internal class FTP_Connection
    {
        /// <summary>
        /// Tries to connect to the ftp server and then goes to either <c>FileUpload</c> or <c>FileDownload</c> to upload/download data
        /// </summary>
        /// <param name="verbose"></param>
        /// <param name="type_of_op"></param>
        /// <exception cref="AccessViolationException"></exception>
        /// <exception cref="FtpException"></exception>
        public void connection_manager(bool verbose, string type_of_op)
        {
            try
            {
                StreamReader file = File.OpenText(".\\Config\\FTP_Config.json");
                Stopwatch stopwatch;
                JsonSerializer serializer = new JsonSerializer();
                FTP_Json? ftp_config = serializer.Deserialize(file, typeof(FTP_Json)) as FTP_Json;

                //check if the class exists if not somethings up with the file
                if (ftp_config == null)
                {
                    throw new AccessViolationException("Configuration file is unredable or corrupted");
                }

                //grab the config from the json file we deserialized before
                FtpClient client = new FtpClient(ftp_config.host, ftp_config.username, ftp_config.password, ftp_config.port);

                Console.WriteLine("Connecting on " + ftp_config.host + " with port " + ftp_config.port);

                //autoconnect to the closest matching ftp server
                var connection_status = client.AutoConnect();

                if (connection_status == null)
                {
                    throw new FtpException("unable to connect to any suitable ftp servers!");
                }
                if (verbose)
                {
                    //if we enabled verbose cmd we output some info
                    Console.WriteLine("FTP is running on " + client.SystemType + " with connection type " + client.ConnectionType.ToString());
                    Console.WriteLine("List of server functions: ");
                    for (int i = 0; i < client.Capabilities.Count; i++)
                    {
                        Console.Write(client.Capabilities[i].ToString() + "    ");
                    }
                    Console.WriteLine("Using ip protocol version " + client.InternetProtocol.ToString());
                    Console.WriteLine("ssl type " + client.SslProtocolActive);
                }
                Thread.Sleep(2000);

                switch (type_of_op)
                {
                    //either upload or download file
                    case "U":
                        FileUpload upload = new FileUpload();
                        stopwatch = upload.upload_file(client);
                        FileInfo fileInfo = new FileInfo(upload.file_path);
                        Console.WriteLine("All done! transferred " + fileInfo.Length + " bytes in " + stopwatch.Elapsed + "!");
                        break;

                    case "D":
                        FileDownload download = new FileDownload();
                        stopwatch = download.download_file(client);
                        Console.WriteLine("All done! downloaded " + download.file_size + " bytes in " + stopwatch.Elapsed + "!");
                        break;

                    default:
                        throw new NotSupportedException();
                        break;
                }
            }catch(NotSupportedException e)
            {
                System.Windows.Forms.MessageBox.Show("Bad command", e.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, e.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Tries to connect to the ftp server and then goes to either <c>FileUpload</c> or <c>FileDownload</c> to upload/download data if we already have a built <c>FtpClient>/c>
        /// </summary>
        /// <param name="client"></param>
        /// <param name="verbose"></param>
        /// <param name="type_of_op"></param>
        public void connection_manager(FtpClient client, bool verbose, string type_of_op)
        {
            try
            {
                Stopwatch upload_time = new Stopwatch();
                FileUpload upload = new FileUpload();

                string confirmation;
                bool overwrite = false;
                bool verify = false;

                Console.WriteLine("Connecting on " + client.Host + " with port " + client.Port);
                if (verbose)
                {
                    Console.WriteLine("FTP is running on " + client.SystemType + " with connection type " + client.ConnectionType);
                    Console.WriteLine("List of server capabilities: ");
                    for (int i = 0; i < client.Capabilities.Count; i++)
                    {
                        Console.Write(client.Capabilities[i].ToString());
                    }
                    Console.WriteLine("Using protocol " + client.InternetProtocol.ToString());
                    Console.WriteLine("is ssl available? " + client.SslProtocolActive);
                }
                Thread.Sleep(2000);

                client.AutoConnect();

                upload_time = upload.upload_file(client);

                FileInfo fileInfo = new FileInfo(upload.file_path);

                Console.WriteLine("All done! transferred " + fileInfo.Length + " bytes in " + upload_time.Elapsed + "!");
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, e.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}