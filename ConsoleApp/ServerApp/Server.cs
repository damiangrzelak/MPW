using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerApp
{
    class Server
    {


        private static readonly int port = 1234;
        private static readonly IPAddress localAddr = IPAddress.Parse("127.0.0.1");
        private static ResourceManager resourceManager;

        static void Main(string[] args)
        {
            Console.WriteLine("Server");
            resourceManager = ResourceManager.Instance;

            //Create server disk if don't exist
            if (resourceManager.CreateDiskSpace())
                StartServer();
        }
    
        private static void StartServer()
        {
            TcpListener tcpListener = null;
            try
            {
                tcpListener = new TcpListener(localAddr, port);
                tcpListener.Start();
                Console.WriteLine("Server is started... {0}::{1}", localAddr, port);
                for (; ; )
                {
                    Console.WriteLine("Waiting for connections...");
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    Thread thread = new Thread(ProcessClient);
                    thread.Start(tcpClient);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
            }
            finally
            {
                if (tcpListener != null)
                    tcpListener.Stop();
            }
        }

        private static void ProcessClient(object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;
            try
            {
                StreamReader reader = new StreamReader(client.GetStream());
                StreamWriter writer = new StreamWriter(client.GetStream());
                String fileName = String.Empty;
                String clientName = String.Empty;
                List<String> userFiles = new List<String>();

                clientName = reader.ReadLine();
                Console.WriteLine("New connection from {0} ...", clientName);
                List<String> files = resourceManager.GetAllUserFiles(clientName);

                if (files != null)
                {
                    Stream stream = client.GetStream();
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(stream, files);
                }

                while (!(fileName = reader.ReadLine()).Equals("Exit") || (fileName == null))
                {
                    Char operation = fileName[0];
                    fileName = fileName.Substring(1);
                    switch (operation)
                    {
                        case 'u':
                            Console.WriteLine("Upload file {0} for user {1}", fileName, clientName);
                            resourceManager.UploadFile(fileName, clientName);
                            break;

                        case 'd':
                            Console.WriteLine("Download file {0} for user {1}", fileName, clientName);
                            resourceManager.DownloadFile(fileName, clientName);
                            break;

                        default:
                            Console.WriteLine("Error while read msg from{0}: {1}", clientName, fileName);
                            break;
                    }


                    //writer.WriteLine("From server -> " + );
                    writer.Flush();
                }

                reader.Close();
                writer.Close();
                client.Close();
                Console.WriteLine("Closing client connection!");
            }
            catch (IOException)
            {
                Console.WriteLine("Problem with client communication. Exiting thread.");
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                }
            }
        }
    }
}
