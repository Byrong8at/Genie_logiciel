using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;

public class saver
{
    private List<Save_work> Save_work = new List<Save_work>();
    public void open_save(string choice)
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
                    Console.WriteLine($"Sauvegarde {i} exécutée.");
                    // Appeler save_path ici si besoin
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
                    Console.WriteLine($"Sauvegarde {num} exécutée.");
                    // Appeler save_path ici si besoin
                }
                else
                {
                    Console.WriteLine($"Entrée invalide : {s}");
                }
            }
        }
        else if (int.TryParse(choice, out int unique))
        {
            Console.WriteLine($"Sauvegarde {unique} exécutée.");
            // Appeler save_path ici si besoin
        }
    }

    public void save_path(string name_path,string path, string path_cible)
    {
        if (File.Exists(path_cible))
        {
            Console.WriteLine("Veuillez ne pas choisir un fichier mais un dossier comme destination.");
            return;
        }
        //faire différence entre le fichier et si c un directory
        if (File.Exists(path)) {
            CopyFile(name_path,path, path_cible);
             }
        else
        {
            CopyDirectory(name_path,path, path_cible,true);
        }
    }

    static void CopyFile(string name_path,string sourceDir, string destDir)
    {
        try
        {
            string fichier = Path.GetFileName(sourceDir);
            string destination = Path.Combine(destDir, fichier);

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

    static void CopyDirectory(string name_path,string sourceDir, string destinationDir, bool recursive)
    {
        var dir = new DirectoryInfo(sourceDir);

        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        DirectoryInfo[] dirs = dir.GetDirectories();
        Directory.CreateDirectory(destinationDir);
        string folderName = dir.Name;//modifier fichier pour qu'il soit sur l'un des 5 saves

        long totalSize = 0;
        var start = DateTime.Now;
        int totalFiles = dir.GetFiles("*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Length;
        int filesCopied = 0;

        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(destinationDir, file.Name);
            file.CopyTo(targetFilePath, true);
            totalSize += file.Length;
        }

        if (recursive)
        {
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(name_path,subDir.FullName, newDestinationDir, true);
                filesCopied++;
                generate_log_state(
                    name: name_path,
                    srcPath: sourceDir,
                    dstPath: destinationDir,
                    state: "ACTIVE",
                    TotalFiles: totalFiles,
                    TotalSize: totalSize,
                    FilesLeft: totalFiles-filesCopied,
                    Progression: (int)((filesCopied / (double)totalFiles) * 100)
                );

            }
        }

        var end = DateTime.Now;
        double totalDuration = (end - start).TotalSeconds;


        saver.generate_log_day(folderName, sourceDir, destinationDir, totalSize, totalDuration);
        generate_log_state(
                    name: folderName,
                    srcPath: "",
                    dstPath: "",
                    state: "END",
                    TotalFiles: 0,
                    TotalSize: 0,
                    FilesLeft: 0 ,
                    Progression: 0
                );

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
