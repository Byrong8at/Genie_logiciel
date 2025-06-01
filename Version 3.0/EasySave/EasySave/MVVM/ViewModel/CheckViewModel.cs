using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Navigation;
using EasySave.Core;
using EasySave.Services;
using EasySave_Logiciel;

namespace EasySave.MVVM.ViewModel;

public class CheckViewModel : Core.ViewModel
{
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

    private string _targetProJobName;
    public string targetProJobName
    {
        get => _targetProJobName;
        set
        {
            _targetProJobName = value;
            OnPropertyChanged();
        }
    }

    private void AddProJob(string proJobName)
    {
        Logiciel.AddLogicielMetier(proJobName);
    }

    private void DeleteProJob(string proJobName)
    {
        Logiciel.RemoveLogicielMetier(proJobName);
    }

    public RelayCommand NavigateHomeCommand { get; set; }
    public RelayCommand NavigateLanguageCommand { get; set; }
    public RelayCommand NavigateExecuteCommand { get; set; }
    public RelayCommand NavigateCreateCommand { get; set; }
    public RelayCommand NavigateOverviewCommand { get; set; }
    public RelayCommand NavigateDeleteCommand { get; set; }
    public RelayCommand NavigateCheckCommand { get; set; }

    public RelayCommand AddProJobCommand { get; set; }

    public RelayCommand DeleteProJobCommand { get; set; }

    public CheckViewModel(INavigationService navService)
    {
        Navigation = navService;
        NavigateHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
        NavigateLanguageCommand = new RelayCommand(o => { Navigation.NavigateTo<LanguageViewModel>(); }, o => true);
        NavigateExecuteCommand = new RelayCommand(o => { Navigation.NavigateTo<ExecuteViewModel>(); }, o => true);
        NavigateCreateCommand = new RelayCommand(o => { Navigation.NavigateTo<CreateViewModel>(); }, o => true);
        NavigateOverviewCommand = new RelayCommand(o => { Navigation.NavigateTo<OverviewViewModel>(); }, o => true);
        NavigateDeleteCommand = new RelayCommand(o => { Navigation.NavigateTo<DeleteViewModel>(); }, o => true);
        NavigateCheckCommand = new RelayCommand(o => { Navigation.NavigateTo<CheckViewModel>(); }, o => true);
        AddProJobCommand = new RelayCommand(o => { 
            AddProJob(targetProJobName);
        }, o => true);
        DeleteProJobCommand = new RelayCommand(o => {
            DeleteProJob(targetProJobName);
        }, o => true);
    }
}
