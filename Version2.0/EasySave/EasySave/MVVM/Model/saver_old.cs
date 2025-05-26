using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using LogLibrary;
using Xml_logger;
using EasySave_Logger;
using EasySave_Logiciel;
using EasySave_Dictionnary;

public class saver
{
    private List<Save_work> Save_work = new List<Save_work>();



    public List<Save_work> Get_Save_Work()
    {
        return Save_work;
    }

    public void Show_backup()
    {

        if (Save_work.Count > 0)
        {
            Console.WriteLine(Dictionnary.GetMessage("list_header"));
            foreach (var save_work in Save_work)
            {
                Console.WriteLine("- " + save_work.Name);
            }
        }
        else
        {
            Console.WriteLine(Dictionnary.GetMessage("no_backup"));
        }
    }

    public void Delete_Save(string save_name)
    {
        var saveToDelete = Save_work.FirstOrDefault(s => s.Name == save_name);

        if (saveToDelete == null)
        {
            Console.WriteLine(Dictionnary.GetMessage("not_found") + save_name);
            return;
        }

        try
        {
            Directory.Delete(saveToDelete.Cible_repertory, true);
            Save_work.Remove(saveToDelete);
            Console.WriteLine(Dictionnary.GetMessage("delete_success", save_name));
        }
        catch (Exception ex)
        {
            Console.WriteLine(Dictionnary.GetMessage("delete_error") + ex.Message);
        }
    }

    public bool Check_save(string filename)
    {
        var saveToDelete = Save_work.FirstOrDefault(s => s.Name == filename);
        return saveToDelete != null;
    }

    public void Open_save(string choice)
    {

        if (string.IsNullOrWhiteSpace(choice) || (!choice.Contains("-") && !choice.Contains(";") && !choice.Contains("*") && !int.TryParse(choice, out _)))
        {
            Console.WriteLine(Dictionnary.GetMessage("invalid_format"));
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
                        if (Dictionnary.GetLangue() == "fr")
                            Console.WriteLine($"Sauvegarde {i} inexistante.");
                        else if (Dictionnary.GetLangue() == "en")
                            Console.WriteLine($"Save {i} does not exist.");
                    }
                    else
                    {
                        var save = Save_work[i - 1];
                        Copy_Backup(save);
                        if (Dictionnary.GetLangue() == "fr")
                        {
                            Console.WriteLine($"Sauvegarde {i} exécutée.");
                        }
                        if (Dictionnary.GetLangue() == "en")
                        {
                            Console.WriteLine($"backup {i} executed.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(Dictionnary.GetMessage("invalid_range"));
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
                    if (Dictionnary.GetLangue() == "fr")
                    {
                        Console.WriteLine($"Sauvegarde {num} exécutée.");
                    }
                    if (Dictionnary.GetLangue() == "en")
                    {
                        Console.WriteLine($"backup {num} executed.");
                    }
                }
                else
                {
                    Console.WriteLine(Dictionnary.GetMessage("invalid_entry") + s);
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
                    string type_log = save.Log_type;

                    if (!Logiciel.LogicielHandler())
                    {
                        if (Dictionnary.GetLangue() == "fr")
                        {
                            Console.WriteLine($"Sauvegarde {s + 1} bloqué.");
                        }
                        if (Dictionnary.GetLangue() == "en")
                        {
                            Console.WriteLine($"backup {s + 1} block.");
                        }
                        if (type_log.ToLower() == "json")
                        {

                            LogGenerator.GenerateLogState(
                                name: save.Name,
                                srcPath: "",
                                dstPath: "",
                                state: "STOP",
                                totalFiles: 0,
                                totalSize: 0,
                                filesLeft: 0,
                                progression: 0
                            );
                        }
                        else
                        {
                            LogGestionnary.GenerateLogState(save.Name, "", "", "STOP", 0, 0, 0, 0);
                        }

                    }
                    else
                    {
                        Copy_Backup(save);
                        if (Dictionnary.GetLangue() == "fr")
                        {
                            Console.WriteLine($"Sauvegarde {s + 1} exécutée.");
                        }
                        if (Dictionnary.GetLangue() == "en")
                        {
                            Console.WriteLine($"backup {s + 1} executed.");
                        }
                    }
                }
            }
            else
            {
                if (Dictionnary.GetLangue() == "fr")
                {
                    Console.WriteLine($"Aucune sauvegarde existante.");
                }
                if (Dictionnary.GetLangue() == "en")
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
                if (Dictionnary.GetLangue() == "fr")
                    Console.WriteLine($"Sauvegarde {unique} inexistante.");
                else if (Dictionnary.GetLangue() == "en")
                    Console.WriteLine($"Save {unique} does not exist.");
                return;
            }
            var save = Save_work[unique - 1];
            Copy_Backup(save);
            if (Dictionnary.GetLangue() == "fr")
            {
                Console.WriteLine($"Sauvegarde {unique} exécutée.");
            }
            if (Dictionnary.GetLangue() == "en")
            {
                Console.WriteLine($"backup {unique} executed.");
            }
        }
    }


    public void Copy_Backup(Save_work save)
    {
        string name_path = save.Name;
        string path = save.Source_repertory;
        string path_cible = save.Cible_repertory;
        string type_save = save.Save_type;
        string type_log= save.Log_type;


        if (File.Exists(path))
        {
            CopyFile(name_path, path, path_cible, type_save, type_log);
        }
        else
        {
            CopyDirectory(name_path, path, path_cible, type_log, true);
        }
    }

    public void Create_backup(string name_path, string path, string path_cible, string log_type)
    {

        if (File.Exists(path_cible))
        {
            Console.WriteLine("Veuillez ne pas choisir un fichier mais un dossier comme destination.");
            return;
        }
        string fullBackupPath = Path.Combine(path_cible, name_path);

        Save_work.Add(new Save_work(name_path, path, fullBackupPath, "full", log_type));


    }
    static void CopyFile(string name_path, string sourceDir, string destDir, string typeSave, string log_type)
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
            var totalDuration = (end - start).TotalSeconds;
            long totalSize = new FileInfo(sourceDir).Length;
            Logger.Log_end(log_type, name_path, sourceDir, backupFolder, totalSize, totalDuration);

           Console.WriteLine($"Fichier copié avec succès en {totalDuration:F2} secondes.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Erreur pendant la copie : " + e.Message);
        }
    }

    static void CopyDirectory(string name_path, string sourceDir, string destinationDir,string type_log, bool recursive)
    {
        try
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            string backupFolder = Path.Combine(destinationDir, name_path);
            Directory.CreateDirectory(backupFolder);

            string folderName = dir.Name;
            DirectoryInfo[] dirs = dir.GetDirectories();

            var start = DateTime.Now;
            FileInfo[] files = dir.GetFiles("*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            int totalFiles = files.Length;
            int filesCopied = 0;
            long totalSize = 0;


            Logger.Log_State(name_path, sourceDir, backupFolder, totalFiles, totalSize, filesCopied, type_log);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(backupFolder, file.Name);
                file.CopyTo(targetFilePath, true);
                totalSize += file.Length;
                filesCopied++;
                Logger.Log_State(name_path, file.FullName, targetFilePath, totalFiles, totalSize, filesCopied, type_log);

            }

            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(backupFolder, subDir.Name);
                    CopyDirectory(name_path, subDir.FullName, newDestinationDir,type_log, true);
                    filesCopied++;
                }
            }

            var end = DateTime.Now;
            double totalDuration = (end - start).TotalSeconds;
            Logger.Log_end(type_log,name_path, sourceDir, backupFolder, totalSize, totalDuration);
            

            Console.WriteLine("Sauvegarde réussie avec succès.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur durant la sauvegarde : " + ex.Message);
        }
    }


}