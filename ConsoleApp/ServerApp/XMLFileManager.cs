using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ServerApp
{
    class XMLFileManager
    {

        public static void CreateNewXmlFile (String pathToFile)
        {
            XmlTextWriter xmlTextWriter = new XmlTextWriter(pathToFile, Encoding.UTF8);
            xmlTextWriter.Formatting = Formatting.Indented;
            xmlTextWriter.WriteStartElement("FilesOnDisk");
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.Close();
        }


    }
}
