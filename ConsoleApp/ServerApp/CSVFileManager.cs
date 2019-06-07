using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerApp
{
    class CSVFileManager
    {
        static ReaderWriterLock readerWriterLock = new ReaderWriterLock();
        private static readonly int timeout = 20000;

        public static void CreateNewCSVFile(String pathToFile)
        {
            StringBuilder content = new StringBuilder();

            content.AppendLine("Client;Files");
            File.AppendAllText(pathToFile, content.ToString());
        }

        public static List<String> GetAllUserFiles(String pathToCSVFile, String username)
        {
            List<String> userFilesList = new List<String>();
            List<String[]> values = new List<string[]>();
            try
            {
                readerWriterLock.AcquireReaderLock(timeout);
                try
                {
                    values = File.ReadLines(pathToCSVFile)
                     .Skip(1)
                     .Select(line => line.Split(';'))
                     .ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR I/O] Get from file exception: " + e);
                }

                finally
                {
                    readerWriterLock.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            String files = values.Where(x => x[0].Equals(username))
                                            .Select(x => x[1])
                                            .FirstOrDefault();
            if (files != null)
                userFilesList = files.Split(',').ToList();

            return (userFilesList.Count > 0) ? userFilesList : null;
        }

        public static void WriteToCSVFile(String pathToCSVFile, String username, String newFileName)
        {
            try
            {
                readerWriterLock.AcquireWriterLock(timeout);
                try
                {
                    string[] lines = new string[0];
                    lines = File.ReadAllLines(pathToCSVFile);

                    StringBuilder newLines = new StringBuilder();
                    bool userExist = false;

                    foreach (var line in lines)
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length == 2 && parts[0].Equals(username))
                        {
                            parts[1] += "," + newFileName;
                            newLines.AppendLine(String.Format("{0};{1}", username, parts[1]));
                            userExist = true;
                        }
                        else
                        {
                            newLines.AppendLine(line.Trim());
                        }
                    }

                    if (!userExist)
                    {
                        newLines.AppendLine(String.Format("{0};{1}", username, newFileName));
                    }

                    File.WriteAllText(pathToCSVFile, newLines.ToString());
                }
                catch (ApplicationException ex)
                {
                    Console.WriteLine("[ERROR I/O] Access to file exception: " + ex);
                }
                finally
                {
                    readerWriterLock.ReleaseWriterLock();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR I/O] Write to file exception: " + ex);
            }
        }
    }
}
