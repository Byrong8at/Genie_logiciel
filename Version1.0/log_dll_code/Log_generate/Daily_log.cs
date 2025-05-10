  using System.Text.Json;

namespace LogLibrary
{
    public class SaveLog
    {
        public string Name_save { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public long FileSize { get; set; }
        public double FileTransferTime { get; set; } // en secondes
        public string Time { get; set; }
    }

    public static class DailyLogGenerator
    {
        public static void GenerateLogDay(string name, string fileSource, string fileTarget, long fileSize, double fileTransferTime)
        {
            string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave", "day_Logs");
            Directory.CreateDirectory(logDirectory);

            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".json";
            string logPath = Path.Combine(logDirectory, fileName);

            var saveLog = new SaveLog
            {
                Name_save = name,
                FileSource = fileSource,
                FileTarget = fileTarget,
                FileSize = fileSize,
                FileTransferTime = fileTransferTime,
                Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            };

            List<SaveLog> logs;

            if (File.Exists(logPath))
            {
                string existing = File.ReadAllText(logPath);
                logs = JsonSerializer.Deserialize<List<SaveLog>>(existing) ?? new List<SaveLog>();
            }
            else
            {
                logs = new List<SaveLog>();
            }

            logs.Add(saveLog);

            string json = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(logPath, json);
        }
    }
}
