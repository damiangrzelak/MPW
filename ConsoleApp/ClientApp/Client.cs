using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        private static readonly String ipAdress= "127.0.0.1";

        private static FileWatcher fileWatcher;

        static void Main(string[] args)
        {
            Console.WriteLine("Client");
            if (args.Length == 2)
            {
                clientName = args[0];
                pathToLocalDirectory += args[1];

                Console.WriteLine("User {0} localDir = {1}", clientName, pathToLocalDirectory);

                if (Directory.Exists(pathToLocalDirectory))
                {
                    Console.WriteLine("Directory for {0}", clientName);
                    try
                    {
                        TcpClient client = new TcpClient(ipAdress, port);
                        StreamReader reader = new StreamReader(client.GetStream());
                        StreamWriter writer = new StreamWriter(client.GetStream());
                        writer.WriteLine(clientName);
                        writer.Flush();
                        String s = String.Empty;

                        fileWatcher = new FileWatcher(pathToLocalDirectory, client);

                        //Start observe client directory
                        fileWatcher.CheckIfNewFileIsOnServer();


                        while (!s.Equals("Exit"))
                        {



                            Console.Write("Enter a string to send to the server: ");
                            s = Console.ReadLine();
                            Console.WriteLine();
                            writer.WriteLine(s);
                            writer.Flush();
                            String server_string = reader.ReadLine();
                            Console.WriteLine(server_string);

                        }

                        //close connection
                        reader.Close();
                        writer.Close();
                        client.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }
                else
                {
                    Console.WriteLine("Błędna lokalizacja folderu!");
                    return ;
                }

            }
        }

    }
}
