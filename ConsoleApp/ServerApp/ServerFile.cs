using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    public class ServerFile
    {
        public String fileName { get; set; }
        public String owner { get; set; }
        public int size { get; set; }
        private Random rnd;

        public ServerFile(string fileName, String owner)
        {
            rnd = new Random();
            this.fileName = fileName;
            this.owner = owner;
            size = rnd.Next(1000, 20000);
            Console.WriteLine("Create file {0}, owner {1} size {2}", fileName, owner, size);
        }
    }
}
