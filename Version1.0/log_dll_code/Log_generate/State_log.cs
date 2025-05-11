using System.Text.Json;

namespace LogLibrary
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

    public static class LogGenerator
{
    public static void GenerateLogState(string name, string srcPath, string dstPath, string state, int totalFiles, long totalSize, int filesLeft, int progression)
    {
        string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave");
        Directory.CreateDirectory(logDirectory);

        string logPath = Path.Combine(logDirectory, "state.json");

        var newLog = new StateLog
        {
            Name = name,
            SourceFilePath = srcPath,
            TargetFilePath = dstPath,
            State = state,
            TotalFilesToCopy = totalFiles,
            TotalFilesSize = totalSize,
            NbFilesLeftToDo = filesLeft,
            Progression = progression
        };

        List<StateLog> logs;

        if (File.Exists(logPath))
        {
            string existing = File.ReadAllText(logPath);
            logs = JsonSerializer.Deserialize<List<StateLog>>(existing) ?? new List<StateLog>();
        }
        else
        {
            logs = new List<StateLog>();
        }

        // Remplacer l'entrée si elle existe déjà
        int index = logs.FindIndex(log => log.Name == name);
        if (index != -1)
        {
            logs[index] = newLog;
        }
        else
        {
            logs.Add(newLog);
        }

        string json = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(logPath, json);
    }
}

}
