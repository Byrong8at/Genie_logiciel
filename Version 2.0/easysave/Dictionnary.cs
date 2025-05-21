using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EasySave_Dictionnary
{
    public static class Dictionnary
    {
        private static string langue = "fr";

        private static Dictionary<string, (string fr, string en)> messages = new()
        {
            ["list_header"] = ("Liste des livres dans la bibliothèque :", "List of books in the library:"),
            ["no_backup"] = ("Aucune Sauvegarde n'a encore été mise en place", "No backup has been set up yet."),
            ["not_found"] = ("Aucune sauvegarde trouvée avec le nom : ", "No backup found with the name: "),
            ["delete_success"] = ("Sauvegarde \"{0}\" supprimée avec succès.", "Backup \"{0}\" successfully deleted."),
            ["delete_error"] = ("Erreur lors de la suppression du dossier : ", "Error while deleting the folder: "),
            ["invalid_range"] = ("Format de plage invalide.", "Invalid range format."),
            ["invalid_entry"] = ("Entrée invalide : ", "Invalid input: "),
            ["invalid_format"] = ("Veuillez rentrer une valeur au format 1-3, 1;3 ou un seul chiffre.", "Please enter a value in the format 1-3, 1;3 or a single number."),
            ["file_as_target"] = ("Veuillez ne pas choisir un fichier mais un dossier comme destination.", "Please choose a folder, not a file, as destination."),
            ["file_copied"] = ("Fichier copié avec succès en {0:F2} secondes.", "File copied successfully in {0:F2} seconds."),
            ["copy_error"] = ("Erreur pendant la copie : ", "Error during file copy: "),
            ["backup_success"] = ("Sauvegarde réussie avec succès.", "Backup completed successfully."),
            ["backup_error"] = ("Erreur durant la sauvegarde : ", "Error during backup: "),
            ["software_running"] = ("Présence de logiciel métier détecté merci de fermer avant de continuer", "Businnes software detected please close it before continuing"),
        };

        public static string GetMessage(string key, params object[] args)
        {
            if (!messages.ContainsKey(key)) return "???";
            string template = (langue == "en") ? messages[key].en : messages[key].fr;
            return string.Format(template, args);
        }

        public static void SetLangue(string lang)
        {
            langue = (lang == "en") ? "en" : "fr";
        }

        public static string GetLangue()
        {
            return langue;
        }


    }
}
