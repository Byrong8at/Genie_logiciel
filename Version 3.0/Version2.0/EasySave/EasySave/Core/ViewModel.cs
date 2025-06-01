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
        currentExitMsg = Controller.GetMessage("exit_msg");
        currentTargetExecuteDesc = Controller.GetMessage("target_execute_desc");
        currentNumber = Controller.GetMessage("number");
        currentName = Controller.GetMessage("name");
        currentTarget = Controller.GetMessage("target");
        currentSource = Controller.GetMessage("source");
        currentLogType = Controller.GetMessage("log_type");
        currentExecute = Controller.GetMessage("execute");
        currentDelete = Controller.GetMessage("delete");
        currentAdd = Controller.GetMessage("add");
        currentNameBackup = Controller.GetMessage("name_backup");
        currentSelectSourcePath = Controller.GetMessage("select_source_path");
        currentSelectTargetPath = Controller.GetMessage("select_target_path");
        currentSelectLogType = Controller.GetMessage("select_log_type");
        currentCreateBackup = Controller.GetMessage("create_backup");
        currentCheckProJob = Controller.GetMessage("check_pro_job");
        currentProJob = Controller.GetMessage("pro_job");
        currentCurrentBackup = Controller.GetMessage("current_backup");
        currentProgress = Controller.GetMessage("progress");
        currentSize = Controller.GetMessage("size");
        currentInputEmpty = Controller.GetMessage("input_empty");
        currentError = Controller.GetMessage("error");
        currentSoftwareRunning = Controller.GetMessage("software_running");
        currentSaveExists = Controller.GetMessage("save_exists");
        currentEnterBackupNameError = Controller.GetMessage("enter_backup_name_error");
        currentNotFound = Controller.GetMessage("not_found");
        currentDeleteSuccess = Controller.GetMessage("delete_success");
        currentDeleteError = Controller.GetMessage("delete_error");
    }

    private void OnProJobChanged()
    {
        currentProJobList = new List<String>(Logiciel.logicielMetierProcessName); // Refresh property when ProJob changes
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

    private string _currentExitMsg = Controller.GetMessage("exit_msg");
    public string currentExitMsg
    {
        get => _currentExitMsg;
        set
        {
            if (_currentExitMsg != value)
            {
                _currentExitMsg = value;
                OnPropertyChanged();
            }
        }
    }


    private string _currentTargetExecuteDesc = Controller.GetMessage("target_execute_desc");
    public string currentTargetExecuteDesc
    {
        get => _currentTargetExecuteDesc;
        set
        {
            if (_currentTargetExecuteDesc != value)
            {
                _currentTargetExecuteDesc = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentNumber = Controller.GetMessage("number");
    public string currentNumber
    {
        get => _currentNumber;
        set
        {
            if (_currentNumber != value)
            {
                _currentNumber = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentName = Controller.GetMessage("name");
    public string currentName
    {
        get => _currentName;
        set
        {
            if (_currentName != value)
            {
                _currentName = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentTarget = Controller.GetMessage("target");
    public string currentTarget
    {
        get => _currentTarget;
        set
        {
            if (_currentTarget != value)
            {
                _currentTarget = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentSource = Controller.GetMessage("source");
    public string currentSource
    {
        get => _currentSource;
        set
        {
            if (_currentSource != value)
            {
                _currentSource = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentLogType = Controller.GetMessage("log_type");
    public string currentLogType
    {
        get => _currentLogType;
        set
        {
            if (_currentLogType != value)
            {
                _currentLogType = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentExecute = Controller.GetMessage("execute");
    public string currentExecute
    {
        get => _currentExecute;
        set
        {
            if (_currentExecute != value)
            {
                _currentExecute = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentDelete = Controller.GetMessage("delete");
    public string currentDelete
    {
        get => _currentDelete;
        set
        {
            if (_currentDelete != value)
            {
                _currentDelete = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentAdd = Controller.GetMessage("add");
    public string currentAdd
    {
        get => _currentAdd;
        set
        {
            if (_currentAdd != value)
            {
                _currentAdd = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentNameBackup = Controller.GetMessage("name_backup");
    public string currentNameBackup
    {
        get => _currentNameBackup;
        set
        {
            if (_currentNameBackup != value)
            {
                _currentNameBackup = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentSelectSourcePath = Controller.GetMessage("select_source_path");
    public string currentSelectSourcePath
    {
        get => _currentSelectSourcePath;
        set
        {
            if (_currentSelectSourcePath != value)
            {
                _currentSelectSourcePath = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentSelectTargetPath = Controller.GetMessage("select_target_path");
    public string currentSelectTargetPath
    {
        get => _currentSelectTargetPath;
        set
        {
            if (_currentSelectTargetPath != value)
            {
                _currentSelectTargetPath = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentSelectLogType = Controller.GetMessage("select_log_type");
    public string currentSelectLogType
    {
        get => _currentSelectLogType;
        set
        {
            if (_currentSelectLogType != value)
            {
                _currentSelectLogType = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentCreateBackup = Controller.GetMessage("create_backup");
    public string currentCreateBackup
    {
        get => _currentCreateBackup;
        set
        {
            if (_currentCreateBackup != value)
            {
                _currentCreateBackup = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentCheckProJob = Controller.GetMessage("check_pro_job");
    public string currentCheckProJob
    {
        get => _currentCheckProJob;
        set
        {
            if (_currentCheckProJob != value)
            {
                _currentCheckProJob = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentProJob = Controller.GetMessage("pro_job");
    public string currentProJob
    {
        get => _currentProJob;
        set
        {
            if (_currentProJob != value)
            {
                _currentProJob = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentCurrentBackup = Controller.GetMessage("current_backup");
    public string currentCurrentBackup
    {
        get => _currentCurrentBackup;
        set
        {
            if (_currentCurrentBackup != value)
            {
                _currentCurrentBackup = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentProgress = Controller.GetMessage("progress");
    public string currentProgress
    {
        get => _currentProgress;
        set
        {
            if (_currentProgress != value)
            {
                _currentProgress = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentSize = Controller.GetMessage("size");
    public string currentSize
    {
        get => _currentSize;
        set
        {
            if (_currentSize != value)
            {
                _currentSize = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentInputEmpty = Controller.GetMessage("input_empty");
    public string currentInputEmpty
    {
        get => _currentInputEmpty;
        set
        {
            if (_currentInputEmpty != value)
            {
                _currentInputEmpty = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentError = Controller.GetMessage("error");
    public string currentError
    {
        get => _currentError;
        set
        {
            if (_currentError != value)
            {
                _currentError = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentSoftwareRunning = Controller.GetMessage("software_running");
    public string currentSoftwareRunning
    {
        get => _currentSoftwareRunning;
        set
        {
            if (_currentSoftwareRunning != value)
            {
                _currentSoftwareRunning = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentSaveExists = Controller.GetMessage("save_exists");
    public string currentSaveExists
    {
        get => _currentSaveExists;
        set
        {
            if (_currentSaveExists != value)
            {
                _currentSaveExists = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentEnterBackupNameError = Controller.GetMessage("enter_backup_name_error");
    public string currentEnterBackupNameError
    {
        get => _currentEnterBackupNameError;
        set
        {
            if (_currentEnterBackupNameError != value)
            {
                _currentEnterBackupNameError = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentNotFound = Controller.GetMessage("not_found");
    public string currentNotFound
    {
        get => _currentNotFound;
        set
        {
            if (_currentNotFound != value)
            {
                _currentNotFound = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentDeleteSuccess = Controller.GetMessage("delete_success");
    public string currentDeleteSuccess
    {
        get => _currentDeleteSuccess;
        set
        {
            if (_currentDeleteSuccess != value)
            {
                _currentDeleteSuccess = value;
                OnPropertyChanged();
            }
        }
    }

    private string _currentDeleteError = Controller.GetMessage("delete_error");
    public string currentDeleteError
    {
        get => _currentDeleteError;
        set
        {
            if (_currentDeleteError != value)
            {
                _currentDeleteError = value;
                OnPropertyChanged();
            }
        }
    }




}
