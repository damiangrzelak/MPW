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
            Console.WriteLine("Write to disk:: {0} file:: {1} owner:: {2}", pathToXMLFile, file.owner, file.fileName);
            //CSVFileManager.WriteToCSVFile(pathToXMLFile, file.owner, file.fileName);
        }

        public List<String> GetAllUserFiles(String username)
        {
            return CSVFileManager.GetAllUserFiles(pathToXMLFile, username);
        }
    }
}
