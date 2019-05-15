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

        private List<DiskManager> diskList = new List<DiskManager>(5);

        private List<ServerFile> filesToUpload = new List<ServerFile>();
        private List<ServerFile> filesToDownload = new List<ServerFile>();

        public void UploadFile(String fileName, String userName)
        {
            filesToUpload.Add(new ServerFile(fileName, userName));
           
            Thread t = new Thread(() => diskList.ElementAt(0).WriteToFile(filesToUpload.ElementAt(0)));
            t.Start();
            t.Join();
        }

        public void DownloadFile(String fileName, String userName)
        {
            filesToDownload.Add(new ServerFile(fileName, userName));
        }



        public List<String> GetAllUserFiles(String userName)
        {
            List<String> files = new List<string>();
            foreach (DiskManager disk in diskList)
            {
                List<String> diskFiles;
                diskFiles = disk.GetAllUserFiles(userName);
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
                { "Disk1\\", "disk1Files.xml"},
                { "Disk2\\", "disk2Files.xml"},
                { "Disk3\\", "disk3Files.xml"},
                { "Disk4\\", "disk4Files.xml"},
                { "Disk5\\", "disk5Files.xml"}
            };

            foreach (KeyValuePair<String, String> disk in diskSpaceMap)
            {
                String pathToDisk = pathToLocalDirectory + disk.Key;
                String pathToFile = pathToDisk + disk.Value;
                try
                {
                    if (!Directory.Exists(pathToDisk))
                    {
                        Console.WriteLine("Create new disc space " + pathToDisk);
                        Directory.CreateDirectory(pathToDisk);
                    }

                    if (!File.Exists(pathToFile))
                    {
                        Console.WriteLine("Create new xml file {0}  for {1}", disk.Value, disk.Key);
                        XMLFileManager.CreateNewXmlFile(pathToFile);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The process failed: {0}", e.ToString());
                    return false;
                }

                diskList.Add(new DiskManager(pathToDisk, pathToFile));
            }

            return true;
        }
    }
}
