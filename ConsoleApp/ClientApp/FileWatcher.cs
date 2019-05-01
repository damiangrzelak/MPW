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

        public FileWatcher(String pathToDirectory, TcpClient client)
        {
            this.pathToDirectory = pathToDirectory;
            this.client = client;

            fileSystemWatcher = new FileSystemWatcher();
        }

       /* public List<String> GetFilesFromLocalDirecory()
        {
            List<String> files = new List<String>();
            files = Directory.GetFiles(pathToDirectory);
        }
        */

        public void CheckIfNewFileIsOnServer()
        {
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
            Console.Write("New file ##############################");
            String str = String.Empty;
            str = "Found new file: " + e.Name;
            writer.WriteLine(str);
            writer.Flush();
        }
    }
}
