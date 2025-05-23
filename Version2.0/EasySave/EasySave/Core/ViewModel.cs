using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySave.MVVM.Model;

namespace EasySave.Core;



public abstract class ViewModel : ObservableObject
{
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

    private List<String> _currentProJobList = saver.logicielMetierProcessName;
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

    public interface IOService
    {
        string OpenFileDialog(string defaultPath);
        Stream OpenFile(string path);
    }

}
