using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerApp
{
    public class ServerFile
    {
        public String fileName { get; set; }
        public String owner { get; set; }
        public int size { get; set; }
        //private FileSizeE fileSizeE;
        public ServerFile(string fileName, String owner)
        {
            this.fileName = fileName;
            this.owner = owner;
            size = new Random().Next(1000, 15000);
            //if (size < 6000) fileSizeE = FileSizeE.SMALL;
            //else if (size >= 6000 && size < 11000) fileSizeE = FileSizeE.MEDIUM;
            //else fileSizeE = FileSizeE.LARGE;

            Console.WriteLine("[INFO SF] New file on server file {0}, owner {1} size {2}", fileName, owner, size);
            Thread.Sleep(100);
        }
    }
}
