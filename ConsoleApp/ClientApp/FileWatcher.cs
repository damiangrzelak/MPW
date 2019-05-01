using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class FileWatcher
    {
        private String pathToDirectory;
        private FileSystemWatcher fileSystemWatcher;
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private List<String> localFiles;

        public FileWatcher(String pathToDirectory, TcpClient client)
        {
            this.pathToDirectory = pathToDirectory;
            this.client = client;

            localFiles = new List<String>();
            fileSystemWatcher = new FileSystemWatcher();
        }

        ~FileWatcher()
        {
            Console.WriteLine("Call to destructor");
            fileSystemWatcher.Changed -= FileSystemWatcher_Created;
            fileSystemWatcher.Dispose();
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

        private void CheckIfNewFileIsOnServer()
        {
            GetExistingFilesFromLocalDirectory();
        }

        private void GetExistingFilesFromLocalDirectory()
        {
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
    }
}
