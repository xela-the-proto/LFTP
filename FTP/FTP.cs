﻿using FluentFTP;
using FTPconsole.FTP;
using Newtonsoft.Json;
using System.Windows.Forms;
using FluentFTP.Model.Functions;
using System;
using System.Diagnostics;

namespace FTP_console.FTP
{
    internal class FTP
    {
        public void connection_manager()
        {
            try
            {
                Stopwatch upload_time = new Stopwatch();
                FileUpload upload = new FileUpload();
                StreamReader file = File.OpenText(".\\Config\\FTP_Config.json");
                
                JsonSerializer serializer = new JsonSerializer();
                FTP_Json ftp_config = (FTP_Json)serializer.Deserialize(file, typeof(FTP_Json));

                FtpClient client = new FtpClient(ftp_config.host, ftp_config.username, ftp_config.password,ftp_config.port);

                string confirmation;
                bool overwrite = false;
                bool verify = false;

                Console.WriteLine("Connecting on " + ftp_config.host + " with port " + ftp_config.port);
                Thread.Sleep(2000);
                /*
                var profiles = client.AutoDetect(new FtpAutoDetectConfig().IncludeImplicit);

                // if any profiles are found, print the code to the console
                if (profiles.Count > 0)
                {
                    var code = profiles[0].ToCode();
                    Console.WriteLine(code);
                }
                */

                client.AutoConnect();

                Console.WriteLine("select file to upload");
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.ShowDialog();

                long size = client.GetFileSize(openFileDialog.FileName);

                Console.WriteLine("Overwrite files with the same name? [Y/N]");
                confirmation = Console.ReadLine();
                confirmation.ToUpper();

                if (confirmation == "Y")
                {
                    overwrite = true;
                }else overwrite = false;

                Console.WriteLine("Verify file integroty after upload? [Y/N]");
                confirmation = Console.ReadLine();
                confirmation.ToUpper();
                if (confirmation == "Y")
                {
                    verify = true;
                }else verify = false;

                Console.WriteLine("uploading...");


               upload_time = upload.upload_file(openFileDialog, client, overwrite, verify);

                Console.WriteLine("All done! transferred" + size + " in " + upload_time.Elapsed + "!");
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message,e.Source,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            

        }
    }
}
/*
    // add this to create and configure the FTP client
   var client = new FtpClient();
   client.LoadProfile(new FtpProfile {
   Host = "localhost",
   Credentials = new NetworkCredential("User1", ""),
   Encryption = FtpEncryptionMode.Auto,
   Protocols = SslProtocols.Tls11 | SslProtocols.Tls12,
   DataConnection = FtpDataConnectionType.EPSV,
   Encoding = System.Text.UTF8Encoding,
   });
   // if you want to accept any certificate then set ValidateAnyCertificate=true and delete the following event handler
   client.ValidateCertificate += new FtpSslValidation(delegate (FtpClient control, FtpSslValidationEventArgs e) {
   // add your logic to test if the SSL certificate is valid (see the FAQ for examples)
   e.Accept = true;
   });
   client.Connect();
 */
