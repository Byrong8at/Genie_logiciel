using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using EasySave.Core;
using EasySave.MVVM.Model;
using EasySave.Services;
using Microsoft.Win32;

namespace EasySave.MVVM.ViewModel;

public class CreateViewModel : Core.ViewModel
{
    public void SelectFolder(string target)
    {
        OpenFolderDialog openFolderDialog = new OpenFolderDialog();
        if (openFolderDialog.ShowDialog() == true)
        {
            string selectedFolderPath = openFolderDialog.FolderName;
            // Process the selected file path as needed
            if (target == "source")
            {
                currentEnteredSourcePath = selectedFolderPath;
            }
            else if (target == "target")
            {
                currentEnteredTargetPath = selectedFolderPath;
            }
        }
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

    private string? _currentEnteredSourcePath;
    public string? currentEnteredSourcePath
    {
        get => _currentEnteredSourcePath;
        set
        {
            _currentEnteredSourcePath = value;
            OnPropertyChanged();
        }
    }

    private string? _currentEnteredTargetPath;
    public string? currentEnteredTargetPath
    {
        get => _currentEnteredTargetPath;
        set
        {
            _currentEnteredTargetPath = value;
            OnPropertyChanged();
        }
    }

    private string? _currentEnteredBackupName;
    public string? currentEnteredBackupName
    {
        get => _currentEnteredBackupName;
        set
        {
            _currentEnteredBackupName = value;
            OnPropertyChanged();
        }
    }

    private string _currentEnteredBackupLogType = "Json";
    public string currentEnteredBackupLogType
    {
        get => _currentEnteredBackupLogType;
        set
        {
            _currentEnteredBackupLogType = value;
            OnPropertyChanged();
        }
    }

    // Method called when the radioButton for LogType is changed.
    private void ChangeCurrentEnteredBackupLogType(string logType)
    {
        currentEnteredBackupLogType = logType;
    }


    private void CreateBackup()
    {
        if (string.IsNullOrEmpty(currentEnteredBackupName) || string.IsNullOrEmpty(currentEnteredSourcePath) || string.IsNullOrEmpty(currentEnteredTargetPath))
        {
            MessageBox.Show(currentInputEmpty, currentError, MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        Controller.BackupCreation(currentEnteredBackupName, currentEnteredSourcePath, currentEnteredTargetPath, currentEnteredBackupLogType);
    }


    public RelayCommand NavigateHomeCommand { get; set; }
    public RelayCommand NavigateLanguageCommand { get; set; }
    public RelayCommand NavigateExecuteCommand { get; set; }
    public RelayCommand NavigateCreateCommand { get; set; }
    public RelayCommand NavigateOverviewCommand { get; set; }
    public RelayCommand NavigateDeleteCommand { get; set; }
    public RelayCommand NavigateCheckCommand { get; set; }

    public RelayCommand SelectFolderCommand_1 { get; set; }
    public RelayCommand SelectFolderCommand_2 { get; set; }

    public RelayCommand RadioButtonJsonCommand { get; set; }
    public RelayCommand RadioButtonXmlCommand { get; set; }

    public RelayCommand CreateBackupCommand { get; set; }

    public CreateViewModel(INavigationService navService)
    {
        Navigation = navService;
        NavigateHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
        NavigateLanguageCommand = new RelayCommand(o => { Navigation.NavigateTo<LanguageViewModel>(); }, o => true);
        NavigateExecuteCommand = new RelayCommand(o => { Navigation.NavigateTo<ExecuteViewModel>(); }, o => true);
        NavigateCreateCommand = new RelayCommand(o => { Navigation.NavigateTo<CreateViewModel>(); }, o => true);
        NavigateOverviewCommand = new RelayCommand(o => { Navigation.NavigateTo<OverviewViewModel>(); }, o => true);
        NavigateDeleteCommand = new RelayCommand(o => { Navigation.NavigateTo<DeleteViewModel>(); }, o => true);
        NavigateCheckCommand = new RelayCommand(o => { Navigation.NavigateTo<CheckViewModel>(); }, o => true);
        SelectFolderCommand_1 = new RelayCommand(o =>
        {
            SelectFolder("source");
        }, o => true);
        SelectFolderCommand_2 = new RelayCommand(o =>
        {
            SelectFolder("target");
        }, o => true);
        RadioButtonJsonCommand = new RelayCommand(o =>
        {
            ChangeCurrentEnteredBackupLogType("Json");
        }, o => true);
        RadioButtonXmlCommand = new RelayCommand(o =>
        {
            ChangeCurrentEnteredBackupLogType("Xml");
        }, o => true);
        CreateBackupCommand = new RelayCommand(o =>
        {
            CreateBackup();
            Navigation.NavigateTo<OverviewViewModel>();
        }, o => true);
    }
}
