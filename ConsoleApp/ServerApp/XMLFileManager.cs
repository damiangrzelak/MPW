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

        public static void ReadFromFile(String pathToFile, String username)
        {
            try
            {
                XElement root = XElement.Load(pathToFile);
                if (root.Descendants("User").Any())
                {
                    IEnumerable<XElement> userFiles = root.Descendants("User").Where(x => x.Attribute("name") != null && x.Attribute("name").Value == username).FirstOrDefault().Descendants();
                    Console.WriteLine("Checking disk {0} for user {1}", pathToFile, username);

                    foreach (XElement f in userFiles)
                        Console.WriteLine("Found file: " + f.Value);

                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Write to file exception: " + e);
            }

        }

        public static void WriteToFile(String pathToFile, String username, String newFileName)
        {

        }


    }
}
