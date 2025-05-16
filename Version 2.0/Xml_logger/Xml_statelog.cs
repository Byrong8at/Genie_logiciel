using System.Xml;

namespace Xml_logger
{
    public class StateLog
    {
        public string Name { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string State { get; set; }
        public int TotalFilesToCopy { get; set; }
        public long TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public int Progression { get; set; }
    }

    public static class LogGestionnary
    {
        public static void GenerateLogState(string name, string srcPath, string dstPath, string state, int totalFiles, long totalSize, int filesLeft, int progression)
        {
            string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave");
            Directory.CreateDirectory(logDirectory);

            string logPath = Path.Combine(logDirectory, "state.xml");

            if (!File.Exists(logPath))
            {
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

            XmlNode existingNode = doc.SelectSingleNode($"/Logs/State[Name='{name}']");
            if (existingNode != null)
            {
                doc.DocumentElement.RemoveChild(existingNode);
            }

            XmlNode logNode = doc.CreateElement("State");

            XmlElement nameElem = doc.CreateElement("Name");
            nameElem.InnerText = name;
            logNode.AppendChild(nameElem);

            XmlElement sourceElem = doc.CreateElement("SourceFilePath");
            sourceElem.InnerText = srcPath;
            logNode.AppendChild(sourceElem);

            XmlElement targetElem = doc.CreateElement("TargetFilePath");
            targetElem.InnerText = dstPath;
            logNode.AppendChild(targetElem);

            XmlElement stateElem = doc.CreateElement("State");
            stateElem.InnerText = state;
            logNode.AppendChild(stateElem);

            XmlElement totalFilesElem = doc.CreateElement("TotalFilesToCopy");
            totalFilesElem.InnerText = totalFiles.ToString();
            logNode.AppendChild(totalFilesElem);

            XmlElement totalSizeElem = doc.CreateElement("TotalFilesSize");
            totalSizeElem.InnerText = totalFiles.ToString();
            logNode.AppendChild(totalSizeElem);

            XmlElement leftFilesElem = doc.CreateElement("NbFilesLeftToDo");
            leftFilesElem.InnerText = filesLeft.ToString();
            logNode.AppendChild(leftFilesElem);

            XmlElement progressElem = doc.CreateElement("Progression");
            progressElem.InnerText = progression.ToString();
            logNode.AppendChild(progressElem);

            XmlElement timeElem = doc.CreateElement("Time");
            timeElem.InnerText = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            logNode.AppendChild(timeElem);

            doc.DocumentElement.AppendChild(logNode);
            doc.Save(logPath);



        }
    }
}
