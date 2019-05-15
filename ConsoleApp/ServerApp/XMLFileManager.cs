using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ServerApp
{
    class XMLFileManager
    {
        public static void CreateNewXmlFile(String pathToFile)
        {
            XmlTextWriter xmlTextWriter = new XmlTextWriter(pathToFile, Encoding.UTF8);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlTextWriter.WriteStartElement("FilesOnDisk");

            //To be remove
            /*if (pathToFile == @"d:\DropboxApp\ServerSpace\Disk1\disk1Files.xml")
            {
                xmlTextWriter.WriteStartElement("User");
                xmlTextWriter.WriteAttributeString("name", "user1");
                xmlTextWriter.WriteStartElement("File");
                xmlTextWriter.WriteString("Plik1.txt");
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteStartElement("File");
                xmlTextWriter.WriteString("Plik2.txt");
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("User");
                xmlTextWriter.WriteAttributeString("name", "user2");
                xmlTextWriter.WriteStartElement("File");
                xmlTextWriter.WriteString("Pliczek1.txt");
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.WriteEndElement();
            }*/

            xmlTextWriter.WriteEndElement();
            xmlTextWriter.Close();
        }

        public static List<String> GetAllFilesForUser(String pathToFile, String username)
        {
            List<String> userFilesList = new List<String>();
            try
            {
                XElement root = XElement.Load(pathToFile);
                //Console.WriteLine("Checking disk {0} for user {1}", pathToFile, username);
                if (root.Descendants("User").Any())
                {
                    IEnumerable<XElement> userFiles = root.Descendants("User").Where(x => x.Attribute("name") != null && x.Attribute("name").Value == username).FirstOrDefault().Descendants();

                    foreach (XElement f in userFiles)
                    {
                        Console.WriteLine("Found file: " + f.Value);
                        userFilesList.Add(f.Value);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Get from file exception: " + e);
            }

            return (userFilesList.Count > 0) ? userFilesList : null;
        }

        public static void WriteToFile(String pathToXMLFile, String username, String newFileName)
        {

        }


    }
}
