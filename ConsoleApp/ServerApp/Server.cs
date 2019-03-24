using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerApp
{
    class Server
    {
        private static String pathToLocalDirectory = @"d:\DropboxApp\ServerSpace\";
        private static List<String> diskSpaceList = new List<String>()
        {
            "Disk1\\",
            "Disk2\\",
            "Disk3\\",
            "Disk4\\",
            "Disk5\\"

        };

        private static readonly int port = 1234;
        private static readonly IPAddress localAddr = IPAddress.Parse("127.0.0.1");


        static void Main(string[] args)
        {
            Console.WriteLine("Server");

            foreach (String disk in diskSpaceList)
            {
                String path = pathToLocalDirectory + disk;
                if (!Directory.Exists(path))
                {
                    Console.WriteLine("Create new disc space " + path);
                    Directory.CreateDirectory(path);
                }
            }
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
                for(; ; )
                {
                    Console.WriteLine("Waiting for connections...");
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    Thread thread = new Thread(ProcessClient);
                    thread.Start(tcpClient);
                }
            }
            catch(Exception err)
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
                String s = String.Empty;
                String clientName = String.Empty;
                clientName = reader.ReadLine();
                Console.WriteLine("New connection from {0} ...", clientName);


                while (!(s = reader.ReadLine()).Equals("Exit") || (s == null))
                {
                    Console.WriteLine("From client -> " + s);
                    writer.WriteLine("From server -> " + s);
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
