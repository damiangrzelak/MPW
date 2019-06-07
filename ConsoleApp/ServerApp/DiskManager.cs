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
            this.thread = new Thread(WriteToFile);
            this.pathToDisk = pathToDisk;
            this.pathToXMLFile = pathToXMLFile;
        }

        private void WriteToFile(object file)
        {
            ServerFile f = (ServerFile)file;
            Console.WriteLine("[DM INFO] Write to disk:: {0} file:: {1} owner:: {2}", pathToXMLFile, f.owner, f.fileName);
            //CSVFileManager.WriteToCSVFile(pathToXMLFile, f.owner, f.fileName);
        }

        public List<String> GetAllUserFiles(String username)
        {
            return CSVFileManager.GetAllUserFiles(pathToXMLFile, username);
        }
    }
}
