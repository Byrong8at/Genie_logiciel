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
using EasySave_Dictionnary;
using easysave_Crypto;

public class Saver
{
    private List<SaveWork> ListSaveWork = new List<SaveWork>();
    public static List<string> ExtensionsAutorisees = new List<string> { ".xlsx", ".pdf", ".docx", "pdf" };


    static Semaphore semaphore = new Semaphore(5, 5); // (initialCount, maxCount)

    private static readonly int Max_SizeFile = 1000 * 1024; //1000 ko
    static readonly object largeFileLock = new(); // Verrou pour gros fichiers
    private static readonly object logLock = new();

    public static bool State_Save = true;
    public static bool Break_Save = true;

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

    public async Task Open_save(string choice)
    {

        if (string.IsNullOrWhiteSpace(choice))
        {
            Console.WriteLine(Controller.GetMessage("invalid_format"));
            return;
        }

        List<SaveWork> savesToExecute = new();

        switch (choice)
        {
            // Cas où la chaîne contient un '*'
            case var _ when choice.Contains("*"):
                if (ListSaveWork.Count > 0)
                {
                    savesToExecute.AddRange(ListSaveWork);
                }
                else
                {
                    switch (Dictionnary.GetLangue())
                    {
                        case "fr":
                            Console.WriteLine($"Aucune sauvegarde existante.");
                            break;
                        case "en":
                            Console.WriteLine($"No backup job found.");
                            break;
                    }
                }
                break;

            default:
                var matchingSave = ListSaveWork.FirstOrDefault(s => s.Name.Equals(choice, StringComparison.OrdinalIgnoreCase));

                if (matchingSave == null)
                {
                    switch (Dictionnary.GetLangue())
                    {
                        case "fr":
                            Console.WriteLine($"Sauvegarde \"{choice}\" inexistante.");
                            break;
                        case "en":
                            Console.WriteLine($"Save \"{choice}\" does not exist.");
                            break;
                    }
                    return;
                }

                savesToExecute.Add(matchingSave);
                break;
        }


        List<Task> tasks = new();
        foreach (var save in savesToExecute)
        {
            tasks.Add(Task.Run(() =>
            {
                semaphore.WaitOne();
                try
                {
                    if (!Logiciel.LogicielHandler())
                    {
                        lock (logLock)
                            Logger.Log_break(save.Log_type, save.Name);
                    }
                    else
                    {
                        Copy_Backup(save);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }

        await Task.WhenAll(tasks);
    }



    public void Copy_Backup(SaveWork save)
    {
        string name_path = save.Name;
        string path = save.Source_repertory;
        string path_cible = save.Cible_repertory;
        string type_log = save.Log_type;


        if (File.Exists(path))
        {
            CopyFile(name_path, path, path_cible,"Full", type_log);
        }
        else
        {
            CopyDirectory(name_path, path, path_cible, type_log, true);
        }
    }

    public static bool Check_extension(string fileName, string extension)
    {
        if (ExtensionsAutorisees.Contains(extension))
        {
            Console.WriteLine($"Cryptage de : {fileName}");
            return true;
        }
        else
        {
            Console.WriteLine($"Copie normale de : {fileName}");
            return false;
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
    static void CopyFile(string name_path, string sourceDir, string destDir, string typeSave, string log_type)
    {
        try
        {
            string fichier = Path.GetFileName(sourceDir);
            string backupFolder = Path.Combine(destDir, name_path);
            Directory.CreateDirectory(backupFolder);

            string destination;

            
            destination = Path.Combine(backupFolder, fichier);
            

            var start = DateTime.Now;
            long File_length = new System.IO.FileInfo(sourceDir).Length;
            if (File_length > Max_SizeFile)
            {
                lock (largeFileLock) // empêche 2 gros fichiers d'être copiés en même temps
                {
                    if (Check_extension(sourceDir, Path.GetExtension(sourceDir).ToLower()))
                    {
                        CryptoManager.EncryptFile(sourceDir, destination);
                    }
                    else
                    {
                        File.Copy(sourceDir, destination, true);
                    }
                }
            }
            else
            {
                if (Check_extension(sourceDir, Path.GetExtension(sourceDir).ToLower()))
                {
                    CryptoManager.EncryptFile(sourceDir, destination);
                }
                else
                {
                    File.Copy(sourceDir, destination, true);
                }
            }
            var end = DateTime.Now;
            var totalDuration = (end - start).TotalSeconds;
            long totalSize = new FileInfo(sourceDir).Length;
            lock (logLock)
            {
                Logger.Log_end(log_type, name_path, sourceDir, backupFolder, totalSize, totalDuration);
            }

            Console.WriteLine($"Fichier copié avec succès en {totalDuration:F2} secondes.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Erreur pendant la copie : " + e.Message);
        }
    }

    static void CopyDirectory(string name_path, string sourceDir, string destinationDir, string type_log, bool recursive, bool isRoot = true)
    {
        List<string> executeAfter = new();

        try
        {
            
                var dir = new DirectoryInfo(sourceDir);
                if (!dir.Exists)
                    throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

                // Seulement au premier appel : on ajoute le nom de la sauvegarde
                string backupFolder = isRoot
                    ? Path.Combine(destinationDir, name_path)
                    : destinationDir;

                Directory.CreateDirectory(backupFolder);

                DirectoryInfo[] dirs = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles("*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                int totalFiles = files.Length;
                int filesCopied = 0;
                long totalSize = 0;
                var start = DateTime.Now;

                lock (logLock)
                {
                    Logger.Log_State(name_path, sourceDir, backupFolder, totalFiles, totalSize, filesCopied, type_log);
                }

                // Fichiers prioritaires
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (!Break_Save)
                    {
                        Break_Save = true;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("Le statut Break_Save a été remis à true.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        });
                    return;
                    }
                    string targetFilePath = Path.Combine(backupFolder, file.Name);
                    string ext = Path.GetExtension(file.FullName).ToLower();

                    if (ext != ".xlsx" && ext != ".pdf")
                    {
                        executeAfter.Add(file.FullName);
                    }
                    else
                    {
                        while (!Logiciel.LogicielHandler())
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                MessageBox.Show("Logiciel detecté.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            }); 
                            Thread.Sleep(2000);

                        }

                        while (!State_Save)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                MessageBox.Show("Le téléchargement est en pause.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            });
                            Thread.Sleep(2000);

                        }

                    long File_length = new System.IO.FileInfo(file.FullName).Length;
                        if (File_length > Max_SizeFile)
                        {
                            lock (largeFileLock) // empêche 2 gros fichiers d'être copiés en même temps
                            {
                                if (Check_extension(file.FullName, Path.GetExtension(ext).ToLower()))
                                {
                                    CryptoManager.EncryptFile(file.FullName, targetFilePath);
                                }
                                else
                                {
                                    file.CopyTo(targetFilePath, true);
                                }
                            }
                        }
                        else
                        {
                            if (Check_extension(file.FullName, Path.GetExtension(ext).ToLower()))
                            {
                                CryptoManager.EncryptFile(file.FullName, targetFilePath);
                            }
                            else
                            {
                                file.CopyTo(targetFilePath, true);
                            }
                        }
                        totalSize += file.Length;
                        filesCopied++;

                        lock (logLock)
                        {
                            Logger.Log_State(name_path, file.FullName, targetFilePath, totalFiles, totalSize, filesCopied, type_log);
                        }
                    }
                }

                // Recopie des sous-dossiers
                if (recursive)
                {
                    foreach (DirectoryInfo subDir in dirs)
                    {
                        string newDestinationDir = Path.Combine(backupFolder, subDir.Name);
                        CopyDirectory(name_path, subDir.FullName, newDestinationDir, type_log, true, false);
                    }
                }

                // Fichiers à copier après
                foreach (string filePath in executeAfter)
                {
                    if (!Break_Save)
                    {
                        Break_Save = true;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("Le statut Break_Save a été remis à true.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        });
                        return;
                    }
                    FileInfo file = new FileInfo(filePath);
                    string targetFilePath = Path.Combine(backupFolder, file.Name);

                    long File_length = new System.IO.FileInfo(file.FullName).Length;
                    while (!Logiciel.LogicielHandler())
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("Logiciel detecté.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }); 
                        Thread.Sleep(2000);
                    }

                    while (!State_Save)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("Le téléchargement est en pause.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        });
                        Thread.Sleep(2000);

                    }
                if (File_length > Max_SizeFile)
                    {
                        lock (largeFileLock) // empêche 2 gros fichiers d'être copiés en même temps
                        {

                            if (Check_extension(file.FullName, Path.GetExtension(file.FullName).ToLower()))
                            {
                                CryptoManager.EncryptFile(file.FullName, targetFilePath);
                            }
                            else
                            {
                                file.CopyTo(targetFilePath, true);
                            }
                        }
                    }
                    else
                    {

                        if (Check_extension(file.FullName, Path.GetExtension(file.FullName).ToLower()))
                        {
                            CryptoManager.EncryptFile(file.FullName, targetFilePath);
                        }
                        else
                        {
                            file.CopyTo(targetFilePath, true);
                        }
                    }
                    totalSize += file.Length;
                    filesCopied++;

                    lock (logLock)
                    {
                        Logger.Log_State(name_path, file.FullName, targetFilePath, totalFiles, totalSize, filesCopied, type_log);
                    }
                }

                var end = DateTime.Now;
                double totalDuration = (end - start).TotalSeconds;

                lock (logLock)
                {
                    Logger.Log_end(type_log, name_path, sourceDir, backupFolder, totalSize, totalDuration);
                }

                Console.WriteLine("Sauvegarde réussie avec succès.");
            return;   
        }
        
        catch (Exception ex)
        {
            Console.WriteLine("Erreur durant la sauvegarde : " + ex.Message);
        }
    }


}