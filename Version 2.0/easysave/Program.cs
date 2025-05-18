using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xml_logger;


class Program
{
    static string langueActuelle = "fr";

    static Dictionary<string, (string fr, string en)> messages = new Dictionary<string, (string, string)>()
    {
        ["menu_title"] = ("===== Menu Bibliothèque =====", "===== Library Menu ====="),
        ["menu_1"] = ("1. Choisir la langue", "1. Choose language"),
        ["menu_2"] = ("2. Exécuter la sauvegarde", "2. Execute the backup"),
        ["menu_3"] = ("3. Créer une sauvegarde", "3. Create a backup"),
        ["menu_4"] = ("4. Voir les Sauvegardes en cours", "4. View active backups"),
        ["menu_5"] = ("5. Supprimer une sauvegarde", "5. Delete a backup"),
        ["menu_6"] = ("6. Logiciels métiers", "6. Business softwares"),
        ["menu_7"] = ("7. Quitter", "7. Quit"),
        ["menu_input"] = ("Votre choix : ", "Your choice: "),
        ["exit_msg"] = ("Merci d'avoir utilisé le gestionnaire de bibliothèque.", "Thank you for using the library manager."),
        ["invalid_choice"] = ("Choix non valide, veuillez réessayer.", "Invalid choice, please try again."),
        ["press_continue"] = ("\nAppuyez sur une touche pour continuer...", "\nPress any key to continue..."),
        ["choose_lang"] = ("Choisissez la langue souhaitée (fr/en) : ", "Choose your language (fr/en): "),
        ["lang_fr"] = ("Langue définie sur le français.", "Language set to French."),
        ["lang_en"] = ("Langue définie sur l'anglais.", "Language set to English."),
        ["lang_inconnu"] = ("Langue inconnue. Aucune modification.", "Unknown language. No change."),
        ["too_many_saves"] = ("Vous ne pouvez pas créer plus de 5 sauvegardes. Supprimez-en une avant d'en ajouter une nouvelle.", "You cannot create more than 5 backups. Delete one before adding another."),
        ["save_name_prompt"] = ("Nom de la sauvegarde : ", "Backup name: "),
        ["source_path_prompt"] = ("Chemin du fichier à sauvegarder : ", "Path of the file to back up: "),
        ["target_path_prompt"] = ("Chemin du dossier de destination : ", "Destination folder path: "),
        ["save_type_prompt"] = ("Choisir le type de sauvegarde (Séquentielle ou complète) : ", "Choose the type of backup (Sequential or Full) "),
        ["save_type_error"] = ("Veuillez rentrer le type correctement: ", "Please write correclty "),
        ["paths_empty"] = ("Les chemins ne peuvent pas être vides.", "Paths cannot be empty."),
        ["save_type_error"] = ("Veuillez rentrer le type correctement: ", "Please write correclty "),
        ["log_choice_prompt"] = ("Choissisez un type de log entre Json et XML : ", "Choose log type between Json and XML : "),
        ["save_exists"] = ("Ce nom de sauvegarde existe déjà, choisissez-en un autre :", "This backup name already exists, please choose another one:"),
        ["enter_save_to_delete"] = ("Entrez le nom exact de la sauvegarde à supprimer :", "Enter the exact name of the backup to delete:"),
        ["select_save"] = ("Choisissez la ou les sauvegardes souhaitées (ex: 1-3 ou 1;3 ou 1) ou entrez \"*\" pour faire une sauvegarde séquencée : ", "Select the desired backup(s) (e.g., 1-3 or 1;3 or 1) or enter \"*\" to launch a sequenced backup: "),
        ["software_running"] = ("Présence de logiciel métier détecté merci de fermer avant de continuer", "Businnes software detected please close it before continuing"),
        ["user_input"] = ("Entrer le logiciel métier que vous souhaitez bloquer", "Please enter the business software you would like to block "),
        ["software_choice"] = ("Logiciel bloquer", "Software Blocked"),
        ["user_input_add"] = ("Rajouter un logiciel métier interdit", "Add a forbidden business software"),
        ["software_added"] = ("Logiciel rajouter à la liste", "Software added to the list"),
        ["user_input_remove"] = ("Supprimer un logiciel métier interdit", "Remove a forbidden business software"),
        ["software_removed"] = ("Logiciel enlever de la liste", "Software removed from the lsit"),
        ["menu_logiciel1"] = ("1. Ajouter un logiciel métier", "1. Add a business software"),
        ["menu_logiciel2"] = ("2. Retirer un logiciel métier", "1. Remove a business software"),

        ["menu_logiciel3"] = ("3. Vérifier les logiciels métiers", "3. Check business software"),
        ["menu_logiciel4"] = ("4. Quitter", "4. Quit")
    };

    static void Main(string[] args)
    {
        saver saver = new saver();
        bool quitter = false;

        while (!quitter)
        {
            AfficherMenu();

            ConsoleKeyInfo choix = Console.ReadKey();
            Console.Clear();

            switch (choix.KeyChar)
            {
                case '1':
                    Language_choice(); // Modifiée : plus de `saver` ici
                    saver.SetLangue(langueActuelle);
                    break;

                case '2':
                    Save_selection(saver);
                    break;

                case '3':
                    Path_saver(saver);
                    break;

                case '4':
                    Display_save(saver);
                    break;
                case '5':
                    Manage_save(saver);
                    break;

                case '6':
                    askLogicielMetier();
                    break;

                case '7':
                    quitter = true;
                    Console.WriteLine(GetMessage("exit_msg"));
                    break;


                default:
                    Console.WriteLine(GetMessage("invalid_choice"));
                    break;
            }

            Console.WriteLine(GetMessage("press_continue"));
            Console.ReadKey();
            Console.Clear();
        }
    }

    static string GetMessage(string key)
    {
        if (messages.ContainsKey(key))
            return langueActuelle == "en" ? messages[key].en : messages[key].fr;
        return "???";
    }

    static void AfficherMenu()
    {
        Console.WriteLine(GetMessage("menu_title"));
        Console.WriteLine(GetMessage("menu_1"));
        Console.WriteLine(GetMessage("menu_2"));
        Console.WriteLine(GetMessage("menu_3"));
        Console.WriteLine(GetMessage("menu_4"));
        Console.WriteLine(GetMessage("menu_5"));
        Console.WriteLine(GetMessage("menu_6"));
        Console.WriteLine(GetMessage("menu_7"));
        Console.WriteLine("=============================");
        Console.Write(GetMessage("menu_input"));
    }

    static void Language_choice()
    {
        Console.Write(GetMessage("choose_lang"));
        string langue = Console.ReadLine()?.ToLower();

        if (langue == "en")
        {
            langueActuelle = "en";
            Console.WriteLine(GetMessage("lang_en"));
        }
        else if (langue == "fr")
        {
            langueActuelle = "fr";
            Console.WriteLine(GetMessage("lang_fr"));
        }
        else
        {
            Console.WriteLine(GetMessage("lang_inconnu"));
        }
    }

    static void Display_save(saver saver)
    {
        saver.Show_backup();
    }

    static void Manage_save(saver saver)
    {
        saver.Show_backup();
        if (saver.Get_Save_Work().Count > 0)
        {
            Console.WriteLine(GetMessage("enter_save_to_delete"));
            string save_name = Console.ReadLine();
            saver.Delete_Save(save_name);
        }

    }

    private static List<string> logicielMetierProcessName = new List<string> { "explorer.exe", "calc.exe" }; // Liste des logiciels métier 

    public static void AddLogicielMetier(string processName)
    {
        if (!string.IsNullOrEmpty(processName) && !logicielMetierProcessName.Contains(processName))
        {
            logicielMetierProcessName.Add(processName);
        }
    }

    public static void RemoveLogicielMetier(string processName)
    {
        logicielMetierProcessName.Remove(processName);
    }
    //            LogGestionnary.GenerateLogState(name_save, "", "", "STOP", 0, 0, 0, 0);

    public static bool IsLogicielMetier()
    {
        if (logicielMetierProcessName == null || logicielMetierProcessName.Count == 0) return false;

        foreach (string processName in logicielMetierProcessName)
        {
            if (string.IsNullOrEmpty(processName)) continue;

            Process[] processes = Process.GetProcessesByName(processName.Replace(".exe", ""));
            if (processes.Length > 0)
            {
                return true; // Au moins un logiciel métier est en cours
            }
        }
        return false; // Aucun logiciel métier n'est en cours
    }

    public static void askLogicielMetier()
    {
        Console.WriteLine(GetMessage("user_input")); // Nouveau message pour l'utilisateur
        Console.WriteLine(GetMessage("menu_logiciel1"));
        Console.WriteLine(GetMessage("menu_logiciel2"));
        Console.WriteLine(GetMessage("menu_logiciel3"));
        Console.WriteLine(GetMessage("menu_logiciel4"));

        string userInput = Console.ReadLine();

        switch (userInput)
        {
            case "1":
                Console.WriteLine(GetMessage("user_input_add"));
                string addProcessName = Console.ReadLine();
                AddLogicielMetier(addProcessName);
                Console.WriteLine(GetMessage("software_added"));
                askLogicielMetier();
                break;
            case "2":
                Console.WriteLine(GetMessage("user_input_remove"));
                string removeProcessName = Console.ReadLine();
                RemoveLogicielMetier(removeProcessName);
                Console.WriteLine(GetMessage("software_removed"));
                askLogicielMetier();
                break;
            case "3":
                foreach (string processName in logicielMetierProcessName)
                {
                    Console.WriteLine(processName);
                }
                askLogicielMetier();
                break;
            case "4":
                return; // Quitter la méthode
            default:
                Console.WriteLine(GetMessage("invalid_choice"));
                break;
        }
    }

    static void Save_selection(saver saver)
    {
        saver.Show_backup();
        if (saver.Get_Save_Work().Count == 0)
        {
            return;
        }
        Console.Write(GetMessage("select_save"));
        string sauvegarde = Console.ReadLine();
        saver.Open_save(sauvegarde);

        if (IsLogicielMetier())
        {
            Console.WriteLine(GetMessage("software_running"));
            // Enregistrement dand le log necessaire 
            return;
        }
    }

    static void Path_saver(saver saver)
    {
        
       

        Console.Write(GetMessage("save_name_prompt"));
        string save_name = Console.ReadLine();
        if (!saver.Check_save(save_name))
        {
            Console.Write(GetMessage("source_path_prompt"));
            string sourcePath = Console.ReadLine();

            Console.Write(GetMessage("target_path_prompt"));
            string targetPath = Console.ReadLine();

            Console.Write(GetMessage("save_type_prompt"));
            string type_save = Console.ReadLine();

            Console.Write(GetMessage("log_choice_prompt"));
            string log_type = Console.ReadLine();
            while (log_type.ToLower() != "json" && log_type.ToLower() != "xml")
            {
                Console.WriteLine(GetMessage("choice_error_log"));
                Console.Write(GetMessage("log_choice_prompt"));
                log_type = Console.ReadLine();
            }

            if (!string.IsNullOrWhiteSpace(sourcePath) && !string.IsNullOrWhiteSpace(targetPath))
            {
                string typeSaveLower = type_save.ToLower();

                if (typeSaveLower != "complete" && typeSaveLower != "complète" && typeSaveLower != "full" &&
                    typeSaveLower != "séquentielle" && typeSaveLower != "sequentielle" &&
                    typeSaveLower != "sequential")
                {
                    Console.Write(GetMessage("save_type_error"));
                }
                else
                {
                    saver.Create_backup(save_name, sourcePath, targetPath, type_save,log_type);
                }
            }
            else
            {
                Console.WriteLine(GetMessage("paths_empty"));
            }
        }
        else
        {
            Console.WriteLine(GetMessage("save_exists"));
            Path_saver(saver);
        }

    }
}
