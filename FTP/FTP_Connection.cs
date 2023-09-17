using FluentFTP;
using FluentFTP.Exceptions;
using FTP_console.Config;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Windows.Forms;

namespace FTP_console.FTP
{
    internal class FTP_Connection
    {
        public void connection_manager(bool verbose, string type_of_op)
        {
            try
            {
                StreamReader file = File.OpenText(".\\Config\\FTP_Config.json");
                Stopwatch stopwatch = new Stopwatch();
                JsonSerializer serializer = new JsonSerializer();
                FTP_Json? ftp_config = serializer.Deserialize(file, typeof(FTP_Json)) as FTP_Json;

                if (ftp_config == null)
                {
                    throw new AccessViolationException("Configuration file is unredable or corrupted");
                }

                FtpClient client = new FtpClient(ftp_config.host, ftp_config.username, ftp_config.password, ftp_config.port);

                Console.WriteLine("Connecting on " + ftp_config.host + " with port " + ftp_config.port);

                var connection_status = client.AutoConnect();

                if (connection_status == null)
                {
                    throw new FtpException("unable to connect to any suitable ftp servers!");
                }
                if (verbose)
                {
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
                        break;
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, e.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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