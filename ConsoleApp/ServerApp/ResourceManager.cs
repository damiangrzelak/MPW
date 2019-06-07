﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerApp
{
    class ResourceManager
    {
        private static readonly ResourceManager m_oInstance = new ResourceManager();
        public static ResourceManager Instance
        {
            get
            {
                return m_oInstance;
            }
        }

        private static List<DiskManager> diskList = new List<DiskManager>(5);
        protected static object _lock = new object();

        private static List<ServerFile> filesToDownload = new List<ServerFile>();
        private static Queue<ServerFile> smallFileSize = new Queue<ServerFile>();
        private static Queue<ServerFile> mediumFileSize = new Queue<ServerFile>();
        private static Queue<ServerFile> largeFileSize = new Queue<ServerFile>();

        public void UploadFile(String filename, String username)
        {
            lock (_lock)
            {
                ServerFile newFile = new ServerFile(filename, username);
                if (newFile.size < 6) smallFileSize.Enqueue(newFile);
                else if (newFile.size >= 6 && newFile.size < 11) mediumFileSize.Enqueue(newFile);
                else largeFileSize.Enqueue(newFile);
            }
        }

        private static void WriteResourceHandler()
        {
            Console.WriteLine("[INFO RM] Write Resource Handler Start Working");
            while (true)
            {
                foreach (DiskManager d in diskList)
                {
                    if (d.thread.ThreadState == ThreadState.Unstarted)
                    {
                        if (smallFileSize.Count > 0)
                        {
                            d.thread.Start(smallFileSize.Dequeue());
                        }
                        else if (mediumFileSize.Count > 0)
                        {
                            d.thread.Start(mediumFileSize.Dequeue());
                        }
                        else if (largeFileSize.Count > 0)
                        {
                            d.thread.Start(largeFileSize.Dequeue());
                        }
                    }
                }
            }

        }


        public void DownloadFile(String filename, String username)
        {
            ServerFile fileToDownload = new ServerFile(filename, username);

            Thread thread = new Thread(ReadResourceHandler);
            thread.Start(fileToDownload);
            thread.Join();
        }

        private static void ReadResourceHandler(object fileToDownload)
        {
            ServerFile file = (ServerFile)fileToDownload;
            Console.WriteLine("[INFO RM] Download file:: {0} for user::{1}", file.fileName, file.owner);
        }

        public List<String> GetAllUserFiles(String username)
        {
            List<String> files = new List<string>();
            foreach (DiskManager disk in diskList)
            {
                List<String> diskFiles;
                diskFiles = disk.GetAllUserFiles(username);
                if (diskFiles != null)
                {
                    files.AddRange(diskFiles);
                }
            }
            return files;
        }

        public Boolean CreateDiskSpace()
        {
            const String pathToLocalDirectory = @"d:\DropboxApp\ServerSpace\";
            IReadOnlyDictionary<String, String> diskSpaceMap = new Dictionary<String, String>()
            {
                { "Disk1\\", "disk1Files.csv"},
                { "Disk2\\", "disk2Files.csv"},
                { "Disk3\\", "disk3Files.csv"},
                { "Disk4\\", "disk4Files.csv"},
                { "Disk5\\", "disk5Files.csv"}
            };

            foreach (KeyValuePair<String, String> disk in diskSpaceMap)
            {
                String pathToDisk = pathToLocalDirectory + disk.Key;
                String pathToFile = pathToDisk + disk.Value;
                try
                {
                    if (!Directory.Exists(pathToDisk))
                    {
                        Console.WriteLine("[INFO RM] Create new disc space " + pathToDisk);
                        Directory.CreateDirectory(pathToDisk);
                    }

                    if (!File.Exists(pathToFile))
                    {
                        Console.WriteLine("[INFO RM] Create new CSV file {0}  for {1}", disk.Value, disk.Key);
                        CSVFileManager.CreateNewCSVFile(pathToFile);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR RM] The process failed: {0}", e.ToString());
                    return false;
                }

                diskList.Add(new DiskManager(pathToDisk, pathToFile));
            }
            Thread thread = new Thread(WriteResourceHandler);
            thread.Start();
            return true;
        }
    }
}
