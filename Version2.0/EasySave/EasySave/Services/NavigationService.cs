﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.Core;
using EasySave.MVVM.Model;
using EasySave.MVVM.ViewModel;

namespace EasySave.Services;

public interface INavigationService
{
    ViewModel CurrentView { get; }
    void NavigateTo<T>() where T : ViewModel;
}

public class NavigationService : ObservableObject, INavigationService
{
    private readonly Func<Type, ViewModel> _viewModelFactory;
    private ViewModel _currentView;
    

    public ViewModel CurrentView
    {
        get => _currentView;
        private set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public NavigationService(Func<Type, ViewModel> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<TViewModel>() where TViewModel : ViewModel
    {
        ViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
        CurrentView = viewModel;
    }
}