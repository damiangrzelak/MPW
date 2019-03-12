using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    class Client
    {
        private static String clientName;
        private static String pathToLocalDirectory = @"c:\DropboxApp\ClientSpace\";

        static void Main(string[] args)
        {
            Console.WriteLine("Client");
            if (args.Length == 2)
            {
                clientName = args[0];
                pathToLocalDirectory += args[1];

                Console.WriteLine("User {0} localDir = {1}", clientName, pathToLocalDirectory);

                if (Directory.Exists(pathToLocalDirectory))
                {
                    Console.WriteLine("Directory for {0}", clientName);
                }

            }
        }
    }
}
