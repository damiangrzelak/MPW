using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp
{
    class Server
    {
        private static String pathToLocalDirectory = @"d:\DropboxApp\ServerSpace\";
        private static List<String> diskSpaceList = new List<String>()
        {
            "Disk1\\",
            "Disk2\\",
            "Disk3\\",
            "Disk4\\",
            "Disk5\\"

        };

        static void Main(string[] args)
        {
            Console.WriteLine("Server");

            foreach(String disk in diskSpaceList)
            {
                String path = pathToLocalDirectory + disk;
                if (!Directory.Exists(path))
                {
                    Console.WriteLine("Create new disc space " + path);
                    Directory.CreateDirectory(path);
                }
            }

        }
    }
}
