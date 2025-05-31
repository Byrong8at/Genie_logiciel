using EasySaveRemoteConsole.MVVM.Model;
using EasySaveRemoteConsole.Network;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasySaveRemoteConsole.MVVM.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<BackupStatus> Backups { get; set; } = new();

        private readonly SocketClient client = new();

        public MainViewModel()
        {
            client.OnStatusReceived += UpdateBackups;
            _ = client.ConnectAsync();
        }

        private void UpdateBackups(List<BackupStatus> newList)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                Backups.Clear();
                foreach (var item in newList)
                    Backups.Add(item);
            });
        }

        public ICommand PauseCommand => new RelayCommand((param) =>
        {
            if (param is BackupStatus backup)
                _ = client.SendCommand(backup.Name, "pause");
        });

        public ICommand PlayCommand => new RelayCommand((param) =>
        {
            if (param is BackupStatus backup)
                _ = client.SendCommand(backup.Name, "play");
        });

        public ICommand StopCommand => new RelayCommand((param) =>
        {
            if (param is BackupStatus backup)
                _ = client.SendCommand(backup.Name, "stop");
        });

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
