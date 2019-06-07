﻿using System;
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

        public ServerFile(string fileName, String owner)
        {
            this.fileName = fileName;
            this.owner = owner;
            size = new Random().Next(1000, 15000);
            Console.WriteLine("[INFO SF] New file on server file {0}, owner {1} size {2}", fileName, owner, size);
            Thread.Sleep(100);
        }
    }
}
