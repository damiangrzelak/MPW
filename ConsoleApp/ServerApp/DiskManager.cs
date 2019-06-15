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
        public FileSizeE currentFileSize;

        public DiskManager(String pathToDisk, String pathToXMLFile)
        {
            thread = new Thread(WriteToFile);
            this.pathToDisk = pathToDisk;
            this.pathToXMLFile = pathToXMLFile;
            currentFileSize = FileSizeE.FREE;
        }

        public void WriteToFile(object file)
        {
            if (file is ServerFile f)
            {
                Console.WriteLine("[DM INFO] Write to disk:: {0} file:: {1} owner:: {2} size::{3}", pathToXMLFile, f.fileName, f.owner, currentFileSize);
                Thread.Sleep(f.size);
                //Zapis do pliku
                CSVFileManager.WriteToCSVFile(pathToXMLFile, f.owner, f.fileName);
                Console.WriteLine("[DM INFO] File {0} saved.", f.fileName);
                currentFileSize = FileSizeE.FREE;
                thread = new Thread(WriteToFile);
            }
        }

        public List<String> GetAllUserFiles(String username)
        {
            return CSVFileManager.GetAllUserFiles(pathToXMLFile, username);
        }
    }

    public enum FileSizeE
    {
        FREE,
        SMALL,
        MEDIUM,
        LARGE
    }
}
