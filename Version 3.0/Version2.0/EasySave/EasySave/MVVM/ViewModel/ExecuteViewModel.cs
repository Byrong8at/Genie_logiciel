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

public class ExecuteViewModel : Core.ViewModel
{

    private async void ExecuteBackup()
    {
        if (string.IsNullOrEmpty(targetBackupName))
        {
            System.Windows.MessageBox.Show("Please enter a backup number to execute.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            return;
        }

        await Controller.BackupExecutionAsync(targetBackupName);
    }


    private void Click_Pause(object sender, RoutedEventArgs e)
    {
        Saver.State_Save = false;
    }

    private void Click_Start(object sender, RoutedEventArgs e)
    {
        Saver.State_Save = true; 

    }

    private void Click_Break(object sender, RoutedEventArgs e)
    {
        Saver.Break_Save = false;
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

    public RelayCommand Click_PauseCommand { get; set; }
    public RelayCommand Click_StartCommand { get; set; }
    public RelayCommand Click_BreakCommand { get; set; }


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

    public RelayCommand ExecuteBackupCommand { get; set; }
    public ExecuteViewModel(INavigationService navService)
    {
        Navigation = navService;
        NavigateHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
        NavigateLanguageCommand = new RelayCommand(o => { Navigation.NavigateTo<LanguageViewModel>(); }, o => true);
        NavigateExecuteCommand = new RelayCommand(o => { Navigation.NavigateTo<ExecuteViewModel>(); }, o => true);
        NavigateCreateCommand = new RelayCommand(o => { Navigation.NavigateTo<CreateViewModel>(); }, o => true);
        NavigateOverviewCommand = new RelayCommand(o => { Navigation.NavigateTo<OverviewViewModel>(); }, o => true);
        NavigateDeleteCommand = new RelayCommand(o => { Navigation.NavigateTo<DeleteViewModel>(); }, o => true);
        NavigateCheckCommand = new RelayCommand(o => { Navigation.NavigateTo<CheckViewModel>(); }, o => true);
        ExecuteBackupCommand = new RelayCommand(o => { 
            ExecuteBackup(); 
        }, o => true);
        Click_PauseCommand = new RelayCommand(o => Click_Pause(o, null), o => true);
        Click_StartCommand = new RelayCommand(o => Click_Start(o, null), o => true);
        Click_BreakCommand = new RelayCommand(o => Click_Break(o, null), o => true);

    }
}
