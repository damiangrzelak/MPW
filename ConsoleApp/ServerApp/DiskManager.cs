using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerApp
{
    class DiskManager
    {
        private String pathToDisk { get; set; }
        private String pathToXMLFile { get; set; }

        public DiskManager(String pathToDisk, String pathToXMLFile)
        {
            this.pathToDisk = pathToDisk;
            this.pathToXMLFile = pathToXMLFile;
        }

        public void WriteToFile(ServerFile file)
        {
            Console.WriteLine("Sleep for {0}ms", file.size);
            Thread.Sleep(file.size);
            Console.WriteLine("End Sleep");

            XMLFileManager.WriteToFile(pathToXMLFile, file.owner, file.fileName);
        }

        public List<String> GetAllUserFiles(String username)
        {
            return XMLFileManager.GetAllFilesForUser(pathToXMLFile, username);
        }
    }
}
