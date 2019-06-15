using System;
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

        private static List<ServerFile> filesToDownload = new List<ServerFile>();
        private static Queue<ServerFile> smallFileSize = new Queue<ServerFile>();
        private static Queue<ServerFile> mediumFileSize = new Queue<ServerFile>();
        private static Queue<ServerFile> largeFileSize = new Queue<ServerFile>();

        public void UploadFile(String filename, String username)
        {
            ServerFile newFile = new ServerFile(filename, username);
            if (newFile.size < 6000)
            {
                lock (mediumFileSize)
                {
                    smallFileSize.Enqueue(newFile);
                }
            }
            else if (newFile.size >= 6000 && newFile.size < 11000)
            {
                lock (mediumFileSize)
                {
                    mediumFileSize.Enqueue(newFile);
                }
            }
            else
            {
                lock (mediumFileSize)
                {
                    largeFileSize.Enqueue(newFile);
                }
            }
        }

        private static void WriteResourceHandler()
        {
            //Thread.Sleep(5000);
            Console.WriteLine("[INFO RM] Write Resource Handler Start Working");

            while (true)
            {
                foreach (DiskManager d in diskList)
                {
                    if (d.thread.ThreadState == ThreadState.Unstarted)
                    {
                        int smallCount = 0;
                        int mediumCount = 0;
                        int largeCount = 0;
                        CalculateCapacity(ref smallCount, ref mediumCount, ref largeCount);

                        lock (smallFileSize) lock (mediumFileSize) lock (largeFileSize)
                        {
                            if (smallFileSize.Count > 0)
                            {
                                if ((smallCount > 2) && (mediumFileSize.Count > 0 || largeFileSize.Count > 0))
                                {
                                    if (mediumCount < largeCount && (mediumFileSize.Count > 0))
                                    {
                                        RunThread(d, mediumFileSize.Dequeue(), FileSizeE.MEDIUM);
                                    }
                                    else if (mediumCount > largeCount && (largeFileSize.Count > 0))
                                    {
                                        RunThread(d, largeFileSize.Dequeue(), FileSizeE.LARGE);
                                    }
                                    else
                                    {
                                        RunThread(d, smallFileSize.Dequeue(), FileSizeE.SMALL);
                                    }
                                }
                                else
                                {
                                    RunThread(d, smallFileSize.Dequeue(), FileSizeE.SMALL);
                                }
                            }
                            else if (mediumFileSize.Count > 0)
                            {
                                if ((mediumCount > 1) && (largeFileSize.Count > 0))
                                {
                                    RunThread(d, largeFileSize.Dequeue(), FileSizeE.LARGE);
                                }
                                else
                                {
                                    RunThread(d, mediumFileSize.Dequeue(), FileSizeE.MEDIUM);
                                }
                            }
                            else if (largeFileSize.Count > 0)
                            {
                                RunThread(d, largeFileSize.Dequeue(), FileSizeE.LARGE);
                            }
                        }
                    }
                }
            }
        }

        private static void RunThread(DiskManager disk, ServerFile serverFile, FileSizeE fs)
        {
            disk.currentFileSize = fs;
            disk.thread.Start(serverFile);
        }

        private static void CalculateCapacity(ref int smallCount, ref int mediumCount, ref int largeCount)
        {
            foreach (DiskManager fs in diskList)
            {
                switch (fs.currentFileSize)
                {
                    case FileSizeE.SMALL:
                        smallCount++;
                        break;
                    case FileSizeE.MEDIUM:
                        mediumCount++;
                        break;
                    case FileSizeE.LARGE:
                        largeCount++;
                        break;
                    default:
                        break;
                }
            }
        }

        public void DownloadFile(String filename, String username)
        {
            ServerFile fileToDownload = new ServerFile(filename, username);

            Thread thread = new Thread(ReadResourceHandler);
            thread.Start(fileToDownload);
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
