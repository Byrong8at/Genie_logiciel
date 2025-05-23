using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using EasySave.Core;
using EasySave.Services;
using Microsoft.Win32;

namespace EasySave.MVVM.ViewModel;

public class CreateViewModel : Core.ViewModel
{
    public void SelectFolder()
    {
        OpenFolderDialog openFolderDialog = new OpenFolderDialog();
        if (openFolderDialog.ShowDialog() == true)
        {
            string selectedFolderPath = openFolderDialog.FolderName;
            // Process the selected file path as needed
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

    public RelayCommand NavigateHomeCommand { get; set; }
    public RelayCommand NavigateLanguageCommand { get; set; }
    public RelayCommand NavigateExecuteCommand { get; set; }
    public RelayCommand NavigateCreateCommand { get; set; }
    public RelayCommand NavigateOverviewCommand { get; set; }
    public RelayCommand NavigateDeleteCommand { get; set; }
    public RelayCommand NavigateCheckCommand { get; set; }

    public RelayCommand SelectFolderCommand_1 { get; set; }
    public RelayCommand SelectFolderCommand_2 { get; set; }

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
            SelectFolder();
        }, o => true);
        SelectFolderCommand_2 = new RelayCommand(o =>
        {
            SelectFolder();
        }, o => true);
    }
}
