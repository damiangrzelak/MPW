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

        public DiskManager(String pathToDisk, String pathToFile)
        {
            this.pathToDisk = pathToDisk;
            this.pathToFile = pathToFile;
        }

        public Boolean OpenFile()
        {
            throw new NotImplementedException();
        }

        public void WriteToFile(String username, String newFileName)
        {
            XMLFileManager.WriteToFile(pathToFile, username, newFileName);
        }

        public List<String> GetAllUserFiles(String username)
        {
            return XMLFileManager.GetAllFilesForUser(pathToFile, username);
        }

        public void CloseFile()
        {

        }
    }
}
