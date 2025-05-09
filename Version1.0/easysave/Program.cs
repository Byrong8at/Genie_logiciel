using System;
using System.Collections.Generic;

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
        ["menu_6"] = ("6. Quitter", "6. Quit"),
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
        ["paths_empty"] = ("Les chemins ne peuvent pas être vides.", "Paths cannot be empty."),
        ["save_exists"] = ("Ce nom de sauvegarde existe déjà, choisissez-en un autre :", "This backup name already exists, please choose another one:"),
        ["enter_save_to_delete"] = ("Entrez le nom exact de la sauvegarde à supprimer :", "Enter the exact name of the backup to delete:"),
        ["select_save"] = ("Choisissez la ou les sauvegardes souhaitées (ex: 1-3 ou 1;3 ou 1) : ", "Select the desired backup(s) (e.g., 1-3 or 1;3 or 1): ")
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
                    language_choice(); // Modifiée : plus de `saver` ici
                    saver.SetLangue(langueActuelle);
                    break;

                case '2':
                    save_selection(saver);
                    break;

                case '3':
                    path_saver(saver);
                    break;

                case '4':
                    display_save(saver);
                    break;
                case '5':
                    manage_save(saver);
                    break;

                case '6':
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
        Console.WriteLine("=============================");
        Console.Write(GetMessage("menu_input"));
    }

    static void language_choice()
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

    static void display_save(saver saver)
    {
        saver.Show_backup();
    }

    static void manage_save(saver saver)
    {
        saver.Show_backup();
        if (saver.Get_Save_Work().Count > 0)
        {
            Console.WriteLine(GetMessage("enter_save_to_delete"));
            string save_name = Console.ReadLine();
            saver.Delete_Save(save_name);
        }

    }

    static void save_selection(saver saver)
    {
        Console.Write(GetMessage("select_save"));
        string sauvegarde = Console.ReadLine();
        saver.Open_save(sauvegarde);
    }

    static void path_saver(saver saver)
    {
        if (saver.Get_Save_Work().Count >= 5)
        {
            Console.WriteLine(GetMessage("too_many_saves"));
            return;  // Empêche de continuer la création de sauvegarde
        }
        Console.Write(GetMessage("save_name_prompt"));
        string save_name = Console.ReadLine();
        if (!saver.Check_save(save_name))
        {
            Console.Write(GetMessage("source_path_prompt"));
            string sourcePath = Console.ReadLine();

            Console.Write(GetMessage("target_path_prompt"));
            string targetPath = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(sourcePath) && !string.IsNullOrWhiteSpace(targetPath))
            {
                saver.Create_backup(save_name, sourcePath, targetPath);
            }
            else
            {
                Console.WriteLine(GetMessage("paths_empty"));
            }
        }
        else
        {
            Console.WriteLine(GetMessage("save_exists"));
            path_saver(saver);
        }
        
    }
}
