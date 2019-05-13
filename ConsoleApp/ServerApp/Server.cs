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

        private static List<DiskManager> diskList = new List<DiskManager>(5);

        static void Main(string[] args)
        {
            Console.WriteLine("Server");

            if (CreateDiskSpace())
                StartServer();


        }

        private static Boolean CreateDiskSpace()
        {
            const String pathToLocalDirectory = @"d:\DropboxApp\ServerSpace\";
            IReadOnlyDictionary<String, String> diskSpaceMap = new Dictionary<String, String>()
            {
                { "Disk1\\", "disk1Files.xml"},
                { "Disk2\\", "disk2Files.xml"},
                { "Disk3\\", "disk3Files.xml"},
                { "Disk4\\", "disk4Files.xml"},
                { "Disk5\\", "disk5Files.xml"}
            };

            foreach (KeyValuePair<String, String> disk in diskSpaceMap)
            {
                String pathToDisk = pathToLocalDirectory + disk.Key;
                String pathToFile = pathToDisk + disk.Value;
                try
                {
                    if (!Directory.Exists(pathToDisk))
                    {
                        Console.WriteLine("Create new disc space " + pathToDisk);
                        Directory.CreateDirectory(pathToDisk);
                    }

                    if (!File.Exists(pathToFile))
                    {
                        Console.WriteLine("Create new xml file {0}  for {1}", disk.Value, disk.Key);
                        CreateNewXmlFile(pathToFile);
                    }
                }
                catch (Exception e )
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                    return false;
                }

                diskList.Add(new DiskManager(pathToDisk, pathToFile));
            }

            return true;
        }

        private static void CreateNewXmlFile(string pathToFile)
        {
            XMLFileManager.CreateNewXmlFile(pathToFile);
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
                String s = String.Empty;
                String clientName = String.Empty;
                List<String> userFiles = new List<String>();

                clientName = reader.ReadLine();
                Console.WriteLine("New connection from {0} ...", clientName);
                List<String> files = new List<String>();

                foreach (DiskManager disk in diskList)
                {
                    List<String> diskFiles;
                    diskFiles = disk.GetAllUserFiles(clientName);
                    if (diskFiles != null)
                    {
                        files.AddRange(diskFiles);
                    }
                }
                Stream stream = client.GetStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(stream, files);

                while (!(s = reader.ReadLine()).Equals("Exit") || (s == null))
                {
                    Console.WriteLine("From {0} -> " + s, clientName);
                    writer.WriteLine("From server -> " + s);
                    writer.Flush();
                }

                reader.Close();
                writer.Close();
                stream.Close();
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
