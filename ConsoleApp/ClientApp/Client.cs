using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    class Client
    {
        //User configuration
        private static String clientName;
        private static String pathToLocalDirectory = @"d:\DropboxApp\ClientSpace\";

        //Socket configuration
        private static readonly int port = 1234;
        private static readonly String ipAdress = "127.0.0.1";

        private static FileManager fileWatcher;

        static void Main(string[] args)
        {
            String fileName = String.Empty;

            if (args.Length == 2)
            {
                clientName = args[0];
                pathToLocalDirectory += args[1];

                Console.WriteLine("[INFO Client] User {0} localDir = {1}", clientName, pathToLocalDirectory);

                if (Directory.Exists(pathToLocalDirectory))
                {
                    Console.WriteLine("[INFO Client] Directory for {0}", clientName);
                    try
                    {
                        TcpClient client = new TcpClient(ipAdress, port);
                        StreamReader reader = new StreamReader(client.GetStream());
                        StreamWriter writer = new StreamWriter(client.GetStream());
                        writer.WriteLine(clientName);
                        writer.Flush();
                        String s = String.Empty;

                        fileWatcher = new FileManager(pathToLocalDirectory, client);

                        //Start observe client directory
                        fileWatcher.CheckIfNewFileIsInLocalDirectory();
                        fileWatcher.CheckIfNewFileIsOnServer();

                        while (!(fileName = reader.ReadLine()).Equals("Exit") || (fileName == null))
                        {
                            //File from server
                            if (fileName[0] == 'n')
                            {
                                fileName = fileName.Substring(1);
                                fileWatcher.CreateNewFileInLocalDirectory(fileName);
                            }
                            else
                            {
                                Console.WriteLine("[INFO Client] Unexpected message from server");
                            }
                        }

                        //close connection
                        reader.Close();
                        writer.Close();
                        client.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("[Error Client] " + e);
                    }
                }
                else
                {
                    Console.WriteLine("[ERROR Client] Błędna lokalizacja folderu!");
                    return;
                }

            }
        }

    }
}
