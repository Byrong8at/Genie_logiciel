using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using EasySave.Core;
using EasySave.MVVM.Model;
using EasySave.Services;

namespace EasySave.MVVM.ViewModel;

public class LanguageViewModel : Core.ViewModel
{
    private void LanguageChoiceFr() => Controller.Language_choice("fr");
    private void LanguageChoiceEn() => Controller.Language_choice("en");

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

    // Command that Naviagate home after changing language to fr
    public RelayCommand LanguageChoiceFrCommand { get; set; }

    // Command that Naviagate home after changing language to en
    public RelayCommand LanguageChoiceEnCommand { get; set; }

    public LanguageViewModel(INavigationService navService)
    {
        Navigation = navService;
        NavigateHomeCommand = new RelayCommand(o => { Navigation.NavigateTo<HomeViewModel>(); }, o => true);
        NavigateLanguageCommand = new RelayCommand(o => { Navigation.NavigateTo<LanguageViewModel>(); }, o => true);
        NavigateExecuteCommand = new RelayCommand(o => { Navigation.NavigateTo<ExecuteViewModel>(); }, o => true);
        NavigateCreateCommand = new RelayCommand(o => { Navigation.NavigateTo<CreateViewModel>(); }, o => true);
        NavigateOverviewCommand = new RelayCommand(o => { Navigation.NavigateTo<OverviewViewModel>(); }, o => true);
        NavigateDeleteCommand = new RelayCommand(o => { Navigation.NavigateTo<DeleteViewModel>(); }, o => true);
        NavigateCheckCommand = new RelayCommand(o => { Navigation.NavigateTo<CheckViewModel>(); }, o => true);
        LanguageChoiceFrCommand = new RelayCommand(o =>
        {
            LanguageChoiceFr();
            Navigation.NavigateTo<HomeViewModel>();
        }, o => true);
        LanguageChoiceEnCommand = new RelayCommand(o =>
        {
            LanguageChoiceEn();
            Navigation.NavigateTo<HomeViewModel>();
        }, o => true);
    }
}
