using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using EasySave.Core;
using EasySave.Services;

namespace EasySave.MVVM.ViewModel;

public class CreateViewModel : Core.ViewModel
{
    private string _selectedPath_1;
    public string SelectedPath_1
    {
        get { return _selectedPath_1; }
        set { _selectedPath_1 = value; OnPropertyChanged("SelectedPath"); }
    }

    private RelayCommand _openCommand;
    public RelayCommand OpenCommand
    {
        get { return _openCommand; }
        set
        {
            _openCommand = value;
            OnPropertyChanged("OpenCommand");
        }
    }

    private void OpenFile()
    {
        SelectedPath_1 = Navigation.OpenFileDialog(@"C:\");
        if (SelectedPath_1 == null)
        {
            SelectedPath_1 = string.Empty;
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
        OpenCommand = new RelayCommand(o => { OpenFile(); }, o => true);
    }
}
