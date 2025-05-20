using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Versionn_1._9_WPF.Core;
using Versionn_1._9_WPF.Services;

namespace Versionn_1._9_WPF.MVVM.ViewModel;


public class HomeViewModel : Core.ViewModel
{
    private INavigationService _navigation;

    public INavigationService Navigation { 
        get => _navigation;
        set
        {
            _navigation = value;
            OnPropertyChanged();
        } 
    }

    public RelayCommand NavigateLanguageCommand { get; set; }

    public HomeViewModel(INavigationService navigation)
    {
        Navigation = navigation;
        NavigateLanguageCommand = new RelayCommand(o => { Navigation.NavigateTo<LanguageViewModel>(); }, o => true);
    }
}
