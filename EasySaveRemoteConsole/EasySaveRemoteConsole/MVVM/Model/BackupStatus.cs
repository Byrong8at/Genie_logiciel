namespace EasySaveRemoteConsole.MVVM.Model
{
    public class BackupStatus
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public int Progress { get; set; } // Pourcentage 0-100
    }
}
