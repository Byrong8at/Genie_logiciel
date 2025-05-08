using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;

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
            Console.WriteLine("Liste des livres dans la bibliothèque :");
            foreach (var save_work in Save_work)
            {
                Console.WriteLine("- "+save_work.Name);
            }
        }
        else
        {
            Console.WriteLine("Aucune Sauvegarde n'a encore été mise en place");
        }
    }

    public void Delete_Save(string save_name)
    {
        var saveToDelete = Save_work.FirstOrDefault(s => s.Name == save_name);

        if (saveToDelete == null)
        {
            Console.WriteLine($"Aucune sauvegarde trouvée avec le nom : {save_name}");
            return;
        }

        try
        {
            Directory.Delete(saveToDelete.Cible_repertory, true);
            Save_work.Remove(saveToDelete);
            Console.WriteLine($"Sauvegarde \"{save_name}\" supprimée avec succès.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur lors de la suppression du dossier : " + ex.Message);
        }
    }


    public bool Check_save(string filename)
    {
        var saveToDelete = Save_work.FirstOrDefault(s => s.Name == filename);
        return saveToDelete != null;
    }

    public void Open_save(string choice)
    {
        if (string.IsNullOrWhiteSpace(choice) || (!choice.Contains("-") && !choice.Contains(";") && !int.TryParse(choice, out _)))
        {
            Console.WriteLine("Veuillez rentrer une valeur au format 1-3, 1;3 ou un seul chiffre.");
            return;
        }

        if (choice.Contains("-"))
        {
            string[] subs = choice.Split('-');
            if (subs.Length == 2 && int.TryParse(subs[0], out int start) && int.TryParse(subs[1], out int end))
            {
                for (int i = start; i <= end; i++)
                {
                    var save = Save_work[i - 1];
                    Copy_Backup(save);
                }
            }
            else
            {
                Console.WriteLine("Format de plage invalide.");
            }
        }
        else if (choice.Contains(";"))
        {
            string[] subs = choice.Split(';');
            foreach (string s in subs)
            {
                if (int.TryParse(s, out int num))
                {
                    var save = Save_work[num - 1];
                    Copy_Backup(save);
                }
                else
                {
                    Console.WriteLine($"Entrée invalide : {s}");
                }
            }
        }
        else if (int.TryParse(choice, out int unique))
        {
            var save = Save_work[unique - 1];
            Copy_Backup(save);
        }
    }

    public void Copy_Backup(Save_work save)
    {
        string name_path = save.Name;
        string path = save.Source_repertory;
        string path_cible = save.Cible_repertory;

        if (File.Exists(path))
        {
            CopyFile(name_path, path, path_cible);
        }
        else
        {
            CopyDirectory(name_path, path, path_cible, true);
        }
    }

    public void Create_backup(string name_path,string path, string path_cible)
    {
        
        if (File.Exists(path_cible))
        {
            Console.WriteLine("Veuillez ne pas choisir un fichier mais un dossier comme destination.");
            return;
        }
        string fullBackupPath = Path.Combine(path_cible, name_path);

        Save_work.Add(new Save_work(name_path, path, fullBackupPath, "Complete"));//modifié par rapport au type de sauvegarde
        

    }

    static void CopyFile(string name_path,string sourceDir, string destDir)
    {
        try
        {
            string fichier = Path.GetFileName(sourceDir);
            string backupFolder = Path.Combine(destDir, name_path); 
            Directory.CreateDirectory(backupFolder);

            string destination = Path.Combine(backupFolder, fichier);

            var start = DateTime.Now;

            File.Copy(sourceDir, destination, true);

            var end = DateTime.Now;
            var duration = (end - start).TotalSeconds;
            long size = new FileInfo(sourceDir).Length;

            generate_log_day(fichier, sourceDir, destination, size, duration);

            generate_log_state(
            name: name_path,
            srcPath: "",
            dstPath: "",
            state: "END",
            TotalFiles: 0,
            TotalSize: 0,
            FilesLeft: 0,
            Progression: 0
        );

            Console.WriteLine($"Fichier copié avec succès en {duration:F2} secondes.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Erreur pendant la copie : " + e.Message);
        }
    }

    static void CopyDirectory(string name_path, string sourceDir, string destinationDir, bool recursive)
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

            // Copier les fichiers du dossier principal
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(backupFolder, file.Name);
                file.CopyTo(targetFilePath, true);
                totalSize += file.Length;
                filesCopied++;
            }

            // Copier les sous-dossiers
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(backupFolder, subDir.Name);
                    CopyDirectory(subDir.Name, subDir.FullName, newDestinationDir, true);
                    filesCopied++;

                    generate_log_state(
                        name: name_path,
                        srcPath: sourceDir,
                        dstPath: newDestinationDir,
                        state: "ACTIVE",
                        TotalFiles: totalFiles,
                        TotalSize: totalSize,
                        FilesLeft: totalFiles - filesCopied,
                        Progression: (int)((filesCopied / (double)totalFiles) * 100)
                    );
                }
            }

            var end = DateTime.Now;
            double totalDuration = (end - start).TotalSeconds;

            generate_log_day(name_path, sourceDir, backupFolder, totalSize, totalDuration);
            generate_log_state(
                name: name_path,
                srcPath: sourceDir,
                dstPath: backupFolder,
                state: "END",
                TotalFiles: totalFiles,
                TotalSize: totalSize,
                FilesLeft: 0,
                Progression: 100
            );

            Console.WriteLine("Sauvegarde réussie avec succès.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur durant la sauvegarde : " + ex.Message);
        }
    }



    public static void generate_log_day(string name, string fileSource, string fileTarget, long fileSize, double fileTransferTime)
    {
        string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave", "day_Logs");
        Directory.CreateDirectory(logDirectory); 

        string name_file = DateTime.Now.ToString("yyyy-MM-dd") + ".json";
        string logPath = Path.Combine(logDirectory, name_file);

        var saveLog = new SaveLog
        {
            Name_save = name,
            FileSource = fileSource,
            FileTarget = fileTarget,
            FileSize = fileSize,
            FileTransferTime = fileTransferTime,
            Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
        };

        List<SaveLog> logs;

        if (File.Exists(logPath))
        {
            string existing = File.ReadAllText(logPath);
            logs = JsonSerializer.Deserialize<List<SaveLog>>(existing) ?? new List<SaveLog>();
        }
        else
        {
            logs = new List<SaveLog>();
        }

        logs.Add(saveLog);

        string json = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(logPath, json);
    }

    public static void generate_log_state(string name, string srcPath, string dstPath, string state, int TotalFiles, long TotalSize, int FilesLeft, int Progression)
    {
        string logDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EasySave", "state_Logs");
        Directory.CreateDirectory(logDirectory);

        string name_file = "state.json";
        string logPath = Path.Combine(logDirectory, name_file);

        var log = new state_log
        {
            name = name,
            SourceFilePath = srcPath,
            TargetFilePath = dstPath,
            State = state,
            TotalFilesToCopy = TotalFiles,
            TotalFilesSize = TotalSize,
            NbFilesLeftToDo = FilesLeft,
            Progression = Progression
        };

        List<state_log> logs;

        if (File.Exists(logPath))
        {
            string existing = File.ReadAllText(logPath);
            logs = JsonSerializer.Deserialize<List<state_log>>(existing) ?? new List<state_log>();
        }
        else
        {
            logs = new List<state_log>();
        }

        logs.Add(log);

        string json = JsonSerializer.Serialize(logs, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(logPath, json);
    }

}
