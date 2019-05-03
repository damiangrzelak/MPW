using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    class DiskManager
    {
        private String pathToDisk { get; set; }
        private String pathToFile { get; set; }
        private XMLFileManager xmlFileManager;

        public DiskManager(String pathToDisk, String pathToFile)
        {
            this.pathToDisk = pathToDisk;
            this.pathToFile = pathToFile;
        }

        public Boolean OpenFile()
        {
            throw new NotImplementedException();
        }

        public void WriteToFile()
        {

        }

        public void ReadFromFile()
        {

        }

        public void CloseFile()
        {

        }
    }
}
