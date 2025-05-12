using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using LogLibrary;
public class saver
{
    private List<Save_work> Save_work = new List<Save_work>();

    private string langue = "fr";

    private Dictionary<string, (string fr, string en)> messages = new()
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
        ["backup_error"] = ("Erreur durant la sauvegarde : ", "Error during backup: ")
    };

    public List<Save_work> Get_Save_Work()
    {
        return Save_work;
    }
    //fonction qui récupère la valeur pour la langue choisit
    private string GetMessage(string key, params object[] args)
    {
        if (!messages.ContainsKey(key)) return "???";
        string template = (langue == "en") ? messages[key].en : messages[key].fr;
        return string.Format(template, args);
    }
    //fonction qui défini la langue
    public void SetLangue(string lang)
    {
        langue = (lang == "en") ? "en" : "fr";
    }

    //fonction qui montre les backup
    public void Show_backup()
    {
        if (Save_work.Count > 0)
        {
            Console.WriteLine(GetMessage("list_header"));
            foreach (var save_work in Save_work)
            {
                Console.WriteLine("- " + save_work.Name);
            }
        }
        else
        {
            Console.WriteLine(GetMessage("no_backup"));
        }
    }

    //fonction qui supprime une save
    public void Delete_Save(string save_name)
    {
        var saveToDelete = Save_work.FirstOrDefault(s => s.Name == save_name);

        if (saveToDelete == null)
        {
            Console.WriteLine(GetMessage("not_found") + save_name);
            return;
        }

        try
        {
            Directory.Delete(saveToDelete.Cible_repertory, true);
            Save_work.Remove(saveToDelete);
            Console.WriteLine(GetMessage("delete_success", save_name));
        }
        catch (Exception ex)
        {
            Console.WriteLine(GetMessage("delete_error") + ex.Message);
        }
    }

    //fonction qui cherche le nom d'une save
    public bool Check_save(string filename)
    {
        var saveToDelete = Save_work.FirstOrDefault(s => s.Name == filename);
        return saveToDelete != null;
    }

    //permet d'ouvrir une sauvegarde pour l'executer
    public void Open_save(string choice)
    {
        if (string.IsNullOrWhiteSpace(choice) || (!choice.Contains("-") && !choice.Contains(";") && !choice.Contains("*") && !int.TryParse(choice, out _)))
        {
            Console.WriteLine(GetMessage("invalid_format"));
            return;
        }

        if (choice.Contains("-"))
        {
            string[] subs = choice.Split('-');
            if (subs.Length == 2 && int.TryParse(subs[0], out int start) && int.TryParse(subs[1], out int end))
            {
                for (int i = start; i <= end; i++)
                {
                    if ((i - 1) < 0 || (i - 1) >= Save_work.Count)
                    {
                        if (langue == "fr")
                            Console.WriteLine($"Sauvegarde {i} inexistante.");
                        else if (langue == "en")
                            Console.WriteLine($"Save {i} does not exist.");
                    }
                    else {
                        var save = Save_work[i - 1];
                        Copy_Backup(save);
                        if (langue == "fr")
                        {
                            Console.WriteLine($"Sauvegarde {i} exécutée.");
                        }
                        if (langue == "en")
                        {
                            Console.WriteLine($"backup {i} executed.");
                        }
                    } }
            }
            else
            {
                Console.WriteLine(GetMessage("invalid_range"));
            }
        }
        else if (choice.Contains(";"))
        {
            
            string[] subs = choice.Split(';');
            foreach (string s in subs)
            {
                if (int.TryParse(s, out int value) && (value - 1 < 0 || value - 1 >= Save_work.Count))
                {
                    Console.WriteLine($"Save {value} no exist");
                }
                else if (int.TryParse(s, out int num))
                {
                    var save = Save_work[num - 1];
                    Copy_Backup(save);
                    if (langue == "fr")
                    {
                        Console.WriteLine($"Sauvegarde {num} exécutée.");
                    }
                    if (langue == "en")
                    {
                        Console.WriteLine($"backup {num} executed.");
                    }
                }
                else
                {
                    Console.WriteLine(GetMessage("invalid_entry") + s);
                }
            }
        }
        else if (choice.Contains("*"))
        {
            if (Save_work.Count > 0)
            {
                for (int s = 0; s < Save_work.Count; s++)
                {
                    var save = Save_work[s];
                    Copy_Backup(save);
                    if (langue == "fr")
                    {
                        Console.WriteLine($"Sauvegarde {s + 1} exécutée.");
                    }
                    if (langue == "en")
                    {
                        Console.WriteLine($"backup {s + 1} executed.");
                    }
                }
            }
            else
            {
                if (langue == "fr")
                {
                    Console.WriteLine($"Aucune sauvegarde existante.");
                }
                if (langue == "en")
                {
                    Console.WriteLine($"No backup job found.");
                }
                return;
            }
        }
        else if (int.TryParse(choice, out int unique))
        {
            if ((unique - 1) < 0 || (unique - 1) >= Save_work.Count)
            {
                if (langue == "fr")
                    Console.WriteLine($"Sauvegarde {unique} inexistante.");
                else if (langue == "en")
                    Console.WriteLine($"Save {unique} does not exist.");
                return;
            }
            var save = Save_work[unique - 1];
            Copy_Backup(save);
            if (langue == "fr")
            {
                Console.WriteLine($"Sauvegarde {unique} exécutée.");
            }
            if (langue == "en")
            {
                Console.WriteLine($"backup {unique} executed.");
            }
        }
    }

    //fonction gérant quel type de copie
    public void Copy_Backup(Save_work save)
    {
        string name_path = save.Name;
        string path = save.Source_repertory;
        string path_cible = save.Cible_repertory;
        string type_save = save.Save_type;


        if (File.Exists(path))
        {
            CopyFile(name_path, path, path_cible, type_save);
        }
        else
        {
            CopyDirectory(name_path, path, path_cible, true);
        }
    }

    //la creation des travaux de sauvegarde
    public void Create_backup(string name_path, string path, string path_cible, string type_save)
    {

        if (File.Exists(path_cible))
        {
            Console.WriteLine("Veuillez ne pas choisir un fichier mais un dossier comme destination.");
            return;
        }
        string fullBackupPath = Path.Combine(path_cible, name_path);

        Save_work.Add(new Save_work(name_path, path, fullBackupPath, type_save));


    }
    //fonction qui permet la copie de fichier
    static void CopyFile(string name_path, string sourceDir, string destDir, string typeSave)
    {
        try
        {
            string fichier = Path.GetFileName(sourceDir);
            string backupFolder = Path.Combine(destDir, name_path);
            Directory.CreateDirectory(backupFolder);

            string destination;

            if (typeSave.ToLower() == "séquentielle" || typeSave.ToLower() == "sequentielle" || typeSave.ToLower() == "sequential")
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string extension = Path.GetExtension(fichier);
                string filenameWithoutExt = Path.GetFileNameWithoutExtension(fichier);
                string newFileName = $"{filenameWithoutExt}_{timestamp}{extension}";
                destination = Path.Combine(backupFolder, newFileName);
            }
            else
            {
                destination = Path.Combine(backupFolder, fichier);
            }

            var start = DateTime.Now;

            File.Copy(sourceDir, destination, true);

            var end = DateTime.Now;
            var duration = (end - start).TotalSeconds;
            long size = new FileInfo(sourceDir).Length;

            DailyLogGenerator.GenerateLogDay(fichier, sourceDir, destination, size, duration);

            LogGenerator.GenerateLogState(
                name: name_path,
                srcPath: sourceDir,
                dstPath: destination,
                state: "END",
                totalFiles: 1,
                totalSize: size,
                filesLeft: 0,
                progression: 100
            );

            Console.WriteLine($"Fichier copié avec succès en {duration:F2} secondes.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Erreur pendant la copie : " + e.Message);
        }
    }

    //fonction qui gere la copie de dossier
    static void CopyDirectory(string name_path, string sourceDir, string destinationDir, bool recursive)
    {
        try
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            string rootDestination = destinationDir;
            Directory.CreateDirectory(rootDestination);

            var start = DateTime.Now;

            FileInfo[] allFiles = dir.GetFiles("*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            int totalFiles = allFiles.Length;
            long totalSize = 0;
            int filesCopied = 0;

            LogGenerator.GenerateLogState(
                name: name_path,
                srcPath: sourceDir,
                dstPath: rootDestination,
                state: "ACTIVE",
                totalFiles: totalFiles,
                totalSize: totalSize,
                filesLeft: totalFiles,
                progression: 0
            );

            Stack<(DirectoryInfo src, string dst)> dirsToProcess = new Stack<(DirectoryInfo, string)>();
            dirsToProcess.Push((dir, rootDestination));

            while (dirsToProcess.Count > 0)
            {
                var (currentSource, currentDestination) = dirsToProcess.Pop();
                Directory.CreateDirectory(currentDestination);

                foreach (FileInfo file in currentSource.GetFiles())
                {
                    string targetFilePath = Path.Combine(currentDestination, file.Name);
                    file.CopyTo(targetFilePath, true);
                    totalSize += file.Length;
                    filesCopied++;

                    LogGenerator.GenerateLogState(
                        name: name_path,
                        srcPath: file.FullName,
                        dstPath: targetFilePath,
                        state: "ACTIVE",
                        totalFiles: totalFiles,
                        totalSize: totalSize,
                        filesLeft: totalFiles - filesCopied,
                        progression: (int)((filesCopied / (double)totalFiles) * 100)
                    );
                }

                if (recursive)
                {
                    foreach (DirectoryInfo subDir in currentSource.GetDirectories())
                    {
                        string newDestinationDir = Path.Combine(currentDestination, subDir.Name);
                        dirsToProcess.Push((subDir, newDestinationDir));
                    }
                }
            }

            var end = DateTime.Now;
            double totalDuration = (end - start).TotalSeconds;

            DailyLogGenerator.GenerateLogDay(name_path, sourceDir, rootDestination, totalSize, totalDuration);

            LogGenerator.GenerateLogState(
                name: name_path,
                srcPath: "",
                dstPath: "",
                state: "END",
                totalFiles: 0,
                totalSize: 0,
                filesLeft: 0,
                progression: 0
            );

            Console.WriteLine("Sauvegarde réussie avec succès.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur durant la sauvegarde : " + ex.Message);
        }
    }
}