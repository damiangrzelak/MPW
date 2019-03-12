using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    class Client
    {
        private static String clientName;
        private static String pathToLocalDirectory;

        static void Main(string[] args)
        {
            Console.WriteLine("Client");
            if (args.Length == 2)
            {
                clientName = args[0];
                pathToLocalDirectory = args[1];

                Console.WriteLine("User {0} localDir = {1}", clientName, pathToLocalDirectory);

            }
        }
    }
}
