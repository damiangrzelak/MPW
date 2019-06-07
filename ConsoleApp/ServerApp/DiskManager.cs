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
        public Thread thread { get; set; }

        public DiskManager(String pathToDisk, String pathToXMLFile)
        {
            thread = new Thread(WriteToFile);
            this.pathToDisk = pathToDisk;
            this.pathToXMLFile = pathToXMLFile;
        }

        public void WriteToFile(object file)
        {
            ServerFile f = (ServerFile)file;
            Console.WriteLine("[DM INFO] Write to disk:: {0} file:: {1} owner:: {2} size::{3}", pathToXMLFile, f.owner, f.fileName, f.size);
            Thread.Sleep(f.size);
            CSVFileManager.WriteToCSVFile(pathToXMLFile, f.owner, f.fileName);
            Console.WriteLine("[DM INFO] File {0} saved.", f.fileName);
            thread = new Thread(WriteToFile);
        }

        public List<String> GetAllUserFiles(String username)
        {
            return CSVFileManager.GetAllUserFiles(pathToXMLFile, username);
        }
    }
}
