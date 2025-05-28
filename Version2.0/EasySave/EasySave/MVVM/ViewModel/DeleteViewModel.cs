using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using EasySave.Core;
using EasySave.MVVM.Model;
using EasySave.Services;

namespace EasySave.MVVM.ViewModel;

public class DeleteViewModel : Core.ViewModel
{
    private void DeleteBackup()
    {
        if (string.IsNullOrEmpty(targetBackupName))
        {
            MessageBox.Show("Please enter a backup name to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        Controller.BackupDeletion(targetBackupName);
    }

    private INavigationService _navigation;

    public INavigationService Navigation
    {
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        }
    }

    private string _targetBackupName;
    public string targetBackupName
    {
        get => _targetBackupName;
        set
        {
            _targetBackupName = value;
            OnPropertyChanged();
        }
    }


    public RelayCommand NavigateHomeCommand { get; set; }
    public RelayCommand NavigateLanguageCommand { get; set; }
    public RelayCommand NavigateExecuteCommand { get; set; }
    public RelayCommand NavigateCreateCommand { get; set; }
    public RelayCommand NavigateOverviewCommand { get; set; }
    public RelayCommand NavigateDeleteCommand { get; set; }
    public RelayCommand NavigateCheckCommand { get; set; }

    public RelayCommand DeleteBackupCommand { get; set; }

    public DeleteViewModel(INavigationService navService)
    {
        Navigation = navService;
        NavigateHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
        NavigateLanguageCommand = new RelayCommand(o => { Navigation.NavigateTo<LanguageViewModel>(); }, o => true);
        NavigateExecuteCommand = new RelayCommand(o => { Navigation.NavigateTo<ExecuteViewModel>(); }, o => true);
        NavigateCreateCommand = new RelayCommand(o => { Navigation.NavigateTo<CreateViewModel>(); }, o => true);
        NavigateOverviewCommand = new RelayCommand(o => { Navigation.NavigateTo<OverviewViewModel>(); }, o => true);
        NavigateDeleteCommand = new RelayCommand(o => { Navigation.NavigateTo<DeleteViewModel>(); }, o => true);
        NavigateCheckCommand = new RelayCommand(o => { Navigation.NavigateTo<CheckViewModel>(); }, o => true);
        DeleteBackupCommand = new RelayCommand(o => { 
            DeleteBackup();
        }, o => true);
    }
}
