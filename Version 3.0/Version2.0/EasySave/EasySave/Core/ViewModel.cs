using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.MVVM.Model;
using EasySave_Logiciel;

namespace EasySave.Core;



public abstract class ViewModel : ObservableObject
{
    // in your constructor you subscribe:
    public ViewModel()
    {
        Controller.LanguageChanged += OnLanguageChanged; // Subscribe to the event
        Logiciel.ProJobChanged += OnProJobChanged; // Subscribe to the event
    }

    private void OnLanguageChanged()
    {
        currentMenuTitle = Controller.GetMessage("menu_title"); // Refresh property when language changes
        currentMenu1 = Controller.GetMessage("menu_1");
        currentMenu2 = Controller.GetMessage("menu_2");
        currentMenu3 = Controller.GetMessage("menu_3");
        currentMenu4 = Controller.GetMessage("menu_4");
        currentMenu5 = Controller.GetMessage("menu_5");
        currentMenu6 = Controller.GetMessage("menu_6");
        currentMenu7 = Controller.GetMessage("menu_7");

    }

    private void OnProJobChanged()
    {
        currentProJobList = new List<String>(Logiciel.logicielMetierProcessName); // Refresh property when ProJob changes
    }

    private string _currentMenuTitle = Controller.GetMessage("menu_title");
    public string currentMenuTitle
    {
        get => _currentMenuTitle;
        set
        {
            if (_currentMenuTitle != value)
            {
                _currentMenuTitle = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentMenu1 = Controller.GetMessage("menu_1");
    public string currentMenu1
    {
        get => _currentMenu1;
        set
        {
            if (_currentMenu1 != value)
            {
                _currentMenu1 = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentMenu2 = Controller.GetMessage("menu_2");
    public string currentMenu2
    {
        get => _currentMenu2;
        set
        {
            if (_currentMenu2 != value)
            {
                _currentMenu2 = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentMenu3 = Controller.GetMessage("menu_3");
    public string currentMenu3
    {
        get => _currentMenu3;
        set
        {
            if (_currentMenu3 != value)
            {
                _currentMenu3 = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentMenu4 = Controller.GetMessage("menu_4");
    public string currentMenu4
    {
        get => _currentMenu4;
        set
        {
            if (_currentMenu4 != value)
            {
                _currentMenu4 = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentMenu5 = Controller.GetMessage("menu_5");
    public string currentMenu5
    {
        get => _currentMenu5;
        set
        {
            if (_currentMenu5 != value)
            {
                _currentMenu5 = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentMenu6 = Controller.GetMessage("menu_6");
    public string currentMenu6
    {
        get => _currentMenu6;
        set
        {
            if (_currentMenu6 != value)
            {
                _currentMenu6 = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentMenu7 = Controller.GetMessage("menu_7");
    public string currentMenu7
    {
        get => _currentMenu7;
        set
        {
            if (_currentMenu7 != value)
            {
                _currentMenu7 = value;
                OnPropertyChanged();
            }
        }
    }

    private List<String> _currentProJobList = new List<String>(Logiciel.logicielMetierProcessName);
    public List<String> currentProJobList
    {
        get => _currentProJobList;
        set
        {
            if (_currentProJobList != value)
            {
                _currentProJobList = value;
                OnPropertyChanged();
            }
        }
    }



    // List of all current backups
    private List<SaveWork> _listCurrentBackups = Controller.Display_save();
    public List<SaveWork> listCurrentBackups
    {
        get => _listCurrentBackups;
        set
        {
            if (_listCurrentBackups != value)
            {
                _listCurrentBackups = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentLanguageChoice = Controller.langueActuelle;
    public string currentLanguageChoice
    {
        get => _currentLanguageChoice;
        set
        {
            if (_currentLanguageChoice != value)
            {
                Controller.Language_choice(value);
                _currentLanguageChoice = value;
                OnPropertyChanged();
            }
        }
    }
}
