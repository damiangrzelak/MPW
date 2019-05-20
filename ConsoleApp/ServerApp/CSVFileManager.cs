using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ServerApp
{
    class CSVFileManager
    {
        public static void CreateNewCSVFile(String pathToFile)
        {
            StringBuilder content = new StringBuilder();

            content.AppendLine("Client;Files");
            File.AppendAllText(pathToFile, content.ToString());
        }

        public static List<String> GetAllUserFiles(String pathToCSVFile, String username)
        {
            List<String> userFilesList = new List<String>();
            try
            {
                var values = File.ReadLines(pathToCSVFile)
                 .Skip(1)
                 .Select(line => line.Split(';'))
                 .ToList();

                String files = values.Where(x => x[0].Equals(username))
                                    .Select(x => x[1])
                                    .First();
                userFilesList = files.Split(',').ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine("Get from file exception: " + e);
            }

            return (userFilesList.Count > 0) ? userFilesList : null;
        }

        public static void WriteToCSVFile(String pathToCSVFile, String username, String newFileName)
        {
            try
            {
                //TO DO
                var values = File.ReadLines(pathToCSVFile)
                .Skip(1)
                .Select(line => line.Split(';'))
                .ToList();

                String files = values.Where(x => x[0].Equals(username))
                                    .Select(x => x[1])
                                    .FirstOrDefault();
                if (files != null)
                {
                    files += "," + newFileName;
                }
                else
                {
                    StringBuilder content = new StringBuilder();
                    content.AppendLine(username + ";" + newFileName);
                    File.AppendAllText(pathToCSVFile, content.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Write to file exception: " + e);
            }
        }
    }
}
