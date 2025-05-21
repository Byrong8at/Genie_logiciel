using System.Xml;

namespace Xml_logger
{
    //class definissant le contenu du json
    public class SaverLog
    {
        public string Name_save { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public long FileSize { get; set; }
        public double FileTransferTime { get; set; } // en secondes
        public string Time { get; set; }
    }

    
    //fonction créeant les logs
    public static class DailyLogGestionnary
    {
        public static void GenerateXMLDay(string name, string fileSource, string fileTarget, long fileSize, double fileTransferTime)
        {
            string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave", "day_Logs");//crée le chemin d'acces
            Directory.CreateDirectory(logDirectory);

            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".xml";//defini le nom du fichier
            string logPath = Path.Combine(logDirectory, fileName);

            if (!File.Exists(logPath))
            {//Check si le fichier existe, et le créer si il n'existe pas
                using (XmlWriter writer = XmlWriter.Create(logPath, new XmlWriterSettings { Indent = true }))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Logs");
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }

            // Charger le document XML existant
            XmlDocument doc = new XmlDocument();
            doc.Load(logPath);

            XmlNode logNode = doc.CreateElement("Log");
            //charger les informations dans le xml
            XmlElement nameElem = doc.CreateElement("Name_save");
            nameElem.InnerText = name;
            logNode.AppendChild(nameElem);

            XmlElement sourceElem = doc.CreateElement("FileSource");
            sourceElem.InnerText = fileSource;
            logNode.AppendChild(sourceElem);

            XmlElement targetElem = doc.CreateElement("FileTarget");
            targetElem.InnerText = fileTarget;
            logNode.AppendChild(targetElem);

            XmlElement sizeElem = doc.CreateElement("FileSize");
            sizeElem.InnerText = fileSize.ToString();
            logNode.AppendChild(sizeElem);

            XmlElement timeTransferElem = doc.CreateElement("FileTransferTime");
            timeTransferElem.InnerText = fileTransferTime.ToString("F2"); // format avec 2 décimales
            logNode.AppendChild(timeTransferElem);

            XmlElement timeElem = doc.CreateElement("Time");
            timeElem.InnerText = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            logNode.AppendChild(timeElem);

            doc.DocumentElement.AppendChild(logNode);
            doc.Save(logPath);
        }
    }
}
