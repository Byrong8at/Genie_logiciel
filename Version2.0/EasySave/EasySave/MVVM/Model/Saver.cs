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
using System.Collections;
using EasySave.MVVM.Model;
using System.Windows;

public class Saver
{
    private List<SaveWork> ListSaveWork = new List<SaveWork>();



    public List<SaveWork> Get_Save_Work()
    {
        return ListSaveWork;
    }

    public void Show_backup()
    {

        if (ListSaveWork.Count > 0)
        {
            Console.WriteLine(Controller.GetMessage("list_header"));
            foreach (SaveWork element in ListSaveWork)
            {
                Console.WriteLine("- " + element.Name);
            }
        }
        else
        {
            Console.WriteLine(Controller.GetMessage("no_backup"));
        }
    }

    public void Delete_Save(string save_name)
    {
        var saveToDelete = ListSaveWork.FirstOrDefault(s => s.Name == save_name);

        if (saveToDelete == null)
        {
            MessageBox.Show(Controller.GetMessage("not_found") + save_name, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        try
        {
            Directory.Delete(saveToDelete.Cible_repertory, true);
            ListSaveWork.Remove(saveToDelete);
            MessageBox.Show(string.Format(Controller.GetMessage("delete_success"), save_name), "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(Controller.GetMessage("delete_error") + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public bool Check_save(string filename)
    {
        var saveToDelete = ListSaveWork.FirstOrDefault(s => s.Name == filename);
        return saveToDelete != null;
    }

    public void Open_save(string choice)
    {

        if (string.IsNullOrWhiteSpace(choice) || (!choice.Contains("-") && !choice.Contains(";") && !choice.Contains("*") && !int.TryParse(choice, out _)))
        {
            Console.WriteLine(Controller.GetMessage("invalid_format"));
            return;
        }

        if (choice.Contains("-"))
        {
            string[] subs = choice.Split('-');
            if (subs.Length == 2 && int.TryParse(subs[0], out int start) && int.TryParse(subs[1], out int end))
            {
                for (int i = start; i <= end; i++)
                {
                    if ((i - 1) < 0 || (i - 1) >= ListSaveWork.Count)
                    {
                        if (Controller.langueActuelle == "fr")
                            Console.WriteLine($"Sauvegarde {i} inexistante.");
                        else if (Controller.langueActuelle == "en")
                            Console.WriteLine($"Save {i} does not exist.");
                    }
                    else
                    {
                        var save = ListSaveWork[i - 1];
                        Copy_Backup(save);
                        if (Controller.langueActuelle == "fr")
                        {
                            Console.WriteLine($"Sauvegarde {i} exécutée.");
                        }
                        if (Controller.langueActuelle == "en")
                        {
                            Console.WriteLine($"backup {i} executed.");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine(Controller.GetMessage("invalid_range"));
            }
        }
        else if (choice.Contains(";"))
        {

            string[] subs = choice.Split(';');
            foreach (string s in subs)
            {
                if (int.TryParse(s, out int value) && (value - 1 < 0 || value - 1 >= ListSaveWork.Count))
                {
                    Console.WriteLine($"Save {value} no exist");
                }
                else if (int.TryParse(s, out int num))
                {
                    var save = ListSaveWork[num - 1];
                    Copy_Backup(save);
                    if (Controller.langueActuelle == "fr")
                    {
                        Console.WriteLine($"Sauvegarde {num} exécutée.");
                    }
                    if (Controller.langueActuelle == "en")
                    {
                        Console.WriteLine($"backup {num} executed.");
                    }
                }
                else
                {
                    Console.WriteLine(Controller.GetMessage("invalid_entry") + s);
                }
            }
        }
        else if (choice.Contains("*"))
        {
            if (ListSaveWork.Count > 0)
            {
                for (int s = 0; s < ListSaveWork.Count; s++)
                {
                    var save = ListSaveWork[s];
                    string type_log = save.Log_type;

                    if (!Logiciel.LogicielHandler())
                    {
                        if (Controller.langueActuelle == "fr")
                        {
                            Console.WriteLine($"Sauvegarde {s + 1} bloqué.");
                        }
                        if (Controller.langueActuelle == "en")
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
                        if (Controller.langueActuelle == "fr")
                        {
                            Console.WriteLine($"Sauvegarde {s + 1} exécutée.");
                        }
                        if (Controller.langueActuelle == "en")
                        {
                            Console.WriteLine($"backup {s + 1} executed.");
                        }
                    }
                }
            }
            else
            {
                if (Controller.langueActuelle == "fr")
                {
                    Console.WriteLine($"Aucune sauvegarde existante.");
                }
                if (Controller.langueActuelle == "en")
                {
                    Console.WriteLine($"No backup job found.");
                }
                return;
            }
        }
        else if (int.TryParse(choice, out int unique))
        {
            if ((unique - 1) < 0 || (unique - 1) >= ListSaveWork.Count)
            {
                if (Controller.langueActuelle == "fr")
                    Console.WriteLine($"Sauvegarde {unique} inexistante.");
                else if (Controller.langueActuelle == "en")
                    Console.WriteLine($"Save {unique} does not exist.");
                return;
            }
            var save = ListSaveWork[unique - 1];
            Copy_Backup(save);
            if (Controller.langueActuelle == "fr")
            {
                Console.WriteLine($"Sauvegarde {unique} exécutée.");
            }
            if (Controller.langueActuelle == "en")
            {
                Console.WriteLine($"backup {unique} executed.");
            }
        }
    }


    public void Copy_Backup(SaveWork save)
    {
        string name_path = save.Name;
        string path = save.Source_repertory;
        string path_cible = save.Cible_repertory;
        string type_log = save.Log_type;


        if (File.Exists(path))
        {
            CopyFile(name_path, path, path_cible, type_log);
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

        ListSaveWork.Add(new SaveWork(name_path, path, fullBackupPath, log_type));


    }
    static void CopyFile(string name_path, string sourceDir, string destDir, string log_type)
    {
        try
        {
            string fichier = Path.GetFileName(sourceDir);
            string backupFolder = Path.Combine(destDir, name_path);
            Directory.CreateDirectory(backupFolder);

            string destination;

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string extension = Path.GetExtension(fichier);
                string filenameWithoutExt = Path.GetFileNameWithoutExtension(fichier);
                string newFileName = $"{filenameWithoutExt}_{timestamp}{extension}";
                destination = Path.Combine(backupFolder, newFileName);
            
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

    static void CopyDirectory(string name_path, string sourceDir, string destinationDir, string type_log, bool recursive)
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
                    CopyDirectory(name_path, subDir.FullName, newDestinationDir, type_log, true);
                    filesCopied++;
                }
            }

            var end = DateTime.Now;
            double totalDuration = (end - start).TotalSeconds;
            Logger.Log_end(type_log, name_path, sourceDir, backupFolder, totalSize, totalDuration);


            Console.WriteLine("Sauvegarde réussie avec succès.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur durant la sauvegarde : " + ex.Message);
        }
    }


}