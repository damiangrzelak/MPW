using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class FileManager
    {
        private String pathToDirectory;
        private FileSystemWatcher fileSystemWatcher;
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private List<String> localFiles;
        private NetworkStream networkStream;

        public FileManager(String pathToDirectory, TcpClient client)
        {
            this.pathToDirectory = pathToDirectory;
            this.client = client;
            this.networkStream = client.GetStream();
            localFiles = new List<String>();
            fileSystemWatcher = new FileSystemWatcher();
        }

        ~FileManager()
        {
            fileSystemWatcher.Changed -= FileSystemWatcher_Created;
            fileSystemWatcher.Dispose();
            networkStream.Close();
            localFiles.Clear();
        }

        public void CheckIfNewFileIsInLocalDirectory()
        {
            GetExistingFilesFromLocalDirectory();

            reader = new StreamReader(client.GetStream());
            writer = new StreamWriter(client.GetStream());

            // Associate event handlers with the events
            fileSystemWatcher.Created += FileSystemWatcher_Created;
            //fileSystemWatcher.Changed += FileSystemWatcher_Changed;
            //fileSystemWatcher.Deleted += FileSystemWatcher_Deleted;
            //fileSystemWatcher.Renamed += FileSystemWatcher_Renamed;

            // Tell the watcher where to look
            fileSystemWatcher.Path = pathToDirectory;

            // Allows events to fire.
            fileSystemWatcher.EnableRaisingEvents = true;

            Console.WriteLine("Start observe {0}", pathToDirectory);
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            String str = String.Empty;
            writer.WriteLine(e.Name);
            writer.Flush();
            AddNewFileToList(e.Name);
        }

        public void CheckIfNewFileIsOnServer()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            List<String> filesOnServer;

            filesOnServer = (List<string>)formatter.Deserialize(networkStream);
            //foreach (String f in filesOnServer)
            //{
            //    Console.WriteLine("File on server " + f);
            //}

            List<String> filesToDownload = filesOnServer.Except(localFiles).ToList();
            List<String> fielsToUpload = localFiles.Except(filesOnServer).ToList();

            if (filesToDownload != null)
            {
                foreach (String f in filesToDownload)
                    DownloadFile(f);
            }

            if (fielsToUpload != null)
            {
                foreach (String f in fielsToUpload)
                    UploadFile(f);
            }
        }

        private void GetExistingFilesFromLocalDirectory()
        {
            localFiles.Clear();
            DirectoryInfo d = new DirectoryInfo(pathToDirectory);
            FileInfo[] Files = d.GetFiles("*.*");
            foreach (FileInfo file in Files)
            {
                localFiles.Add(file.Name);
            }
            PrintAllFilesFromLocalDirectory();
        }

        private void AddNewFileToList(String fileName)
        {
            localFiles.Add(fileName);
        }

        private void PrintAllFilesFromLocalDirectory()
        {
            Console.WriteLine("Files in local directory: ");
            foreach (String fn in localFiles)
            {
                Console.WriteLine(fn);
            }
        }

        private void UploadFile(String filename)
        {
            Console.WriteLine("Upload file " + filename);
        }

        private void DownloadFile(String filename)
        {
            Console.WriteLine("Download file " + filename);

        }
    }
}
