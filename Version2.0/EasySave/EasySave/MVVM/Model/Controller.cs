using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xml_logger;


namespace EasySave.MVVM.Model;

class Controller
{
    static saver currentSaver = new saver();
    public static string langueActuelle = "fr";

    static Dictionary<string, (string fr, string en)> messages = new Dictionary<string, (string, string)>()
    {
        ["menu_title"] = ("Menu Principal", "Main Menu"),
        ["menu_1"] = ("Langue", "Language"),
        ["menu_2"] = ("Exécuter la/les sauvegarde(s)", "Execute the backup(s)"),
        ["menu_3"] = ("Créer une sauvegarde", "Create a backup"),
        ["menu_4"] = ("Voir la/les sauvegarde(s) en cours", "View active backup(s)"),
        ["menu_5"] = ("Supprimer une sauvegarde", "Delete a backup"),
        ["menu_6"] = ("Configuration vérification logiciels métier", "Business softwares verification configuration"),
        ["menu_7"] = ("Quitter", "Quit"),
        ["exit_msg"] = ("Etes vous sûr de vouloir fermer EasySave ? Certaines sauvegardes peuvent être en cours. Merci de vérifier leurs status avant de continuer.", "Are you sure you want to close EasySave? There could be some backups running. Please check the backup status before continuing."),
        ["invalid_choice"] = ("Choix non valide, veuillez réessayer.", "Invalid choice, please try again."),
        ["press_continue"] = ("\nAppuyez sur une touche pour continuer...", "\nPress any key to continue..."),
        ["choose_lang"] = ("Choisissez la langue souhaitée (fr/en) : ", "Choose your language (fr/en): "),
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

    public static string GetMessage(string key)
    {
        if (messages.ContainsKey(key))
            return langueActuelle == "en" ? messages[key].en : messages[key].fr;
        return "???";
    }


    public static void Language_choice(string choice)
    {
        if (choice == "en")
        {
            langueActuelle = "en";
            currentSaver.SetLangue(langueActuelle);
        }
        else if (choice == "fr")
        {
            langueActuelle = "fr";
            currentSaver.SetLangue(langueActuelle);
        }
    }

    public static List<SaveWork> Display_save(saver saver)
    {
        return saver.Get_Save_Work();
    }

    public static void Manage_save(saver saver)
    {
        saver.Show_backup();
        if (saver.Get_Save_Work().Count > 0)
        {
            Console.WriteLine(GetMessage("enter_save_to_delete"));
            string save_name = Console.ReadLine();
            saver.Delete_Save(save_name);
        }

    }

    public static void askLogicielMetier()
    {

        //currentSaver.logicielMetierProcessName

        string userInput = Console.ReadLine();

        switch (userInput)
        {
            case "1":
                Console.WriteLine(GetMessage("user_input_add"));
                string addProcessName = Console.ReadLine();
                saver.AddLogicielMetier(addProcessName);
                Console.WriteLine(GetMessage("software_added"));
                askLogicielMetier();
                break;
            case "2":
                Console.WriteLine(GetMessage("user_input_remove"));
                string removeProcessName = Console.ReadLine();
                saver.RemoveLogicielMetier(removeProcessName);
                Console.WriteLine(GetMessage("software_removed"));
                askLogicielMetier();
                break;
            case "3":
                foreach (string processName in saver.logicielMetierProcessName)
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

    public static void Save_selection(saver saver,string selectedBackup)
    {
        saver.Open_save(selectedBackup);
    }

    public static void BackupCreation(saver saver, string saveName, string sourcePath, string targetPath, string logType)
    {
        if (saver.IsLogicielMetier())
        {
            Console.WriteLine(GetMessage("software_running"));
            return;
        }

        if (!saver.Check_save(saveName))
        {
            if (!string.IsNullOrWhiteSpace(sourcePath) && !string.IsNullOrWhiteSpace(targetPath))
            {
                saver.Create_backup(saveName, sourcePath, targetPath, logType);
            }
            else
            {
                //Error not handled (no paths)
            }
        }
        else
        {
            //Error not handled (save already exists)
        }

    }
}

