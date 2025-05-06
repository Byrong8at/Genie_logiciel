using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.Json;


public class saver
{
    public void open_save(string choice)
    {
        if (choice.Length == 0 || (!choice.Contains("-") && !choice.Contains(";") && int.TryParse(choice, out _)))
        {
            Console.WriteLine("Veuillez rentrer une valeur au format 1-3 ou 1;3");
            return;
        }
        if (choice.Contains("-"))
        {
            string[] subs = choice.Split('-');
            if (subs.Length == 2 && int.TryParse(subs[0], out int start) && int.TryParse(subs[1], out int end))
                for (int i = start; i <= end; i++)
                {
                    return;
                }
            else
            {
                Console.WriteLine("Vous avez mal écris le format, veuillez ne choisir que deux nom");
            }
        }
        if (choice.Contains(";"))
        {
            string[] subs = choice.Split(';');
            foreach (string s in subs)
            {
                return;
            }
        }
        //voir si c un 231 ou 1f2 adapter
    }

    public void save_path(string path, string path_cible)
    {
        //faire un if si y'a extension .jpg etc, retirer et donc faire:
        //File.Copy(Path.Combine(sourceDir, fName), Path.Combine(backupDir, fName), true);
        if (path_cible.Contains("."))//verifier plutot si c un fichier plutot que si y a un .
        {
            Console.WriteLine("Veuillez ne pas choisir un fichier mais un dossier pour le chemin de destination");
            return;
        }
        if (path.Contains("."))
        {
            string dossier = Path.GetDirectoryName(path);  
            string fichier = Path.GetFileName(path);
            File.Copy(Path.Combine(dossier, fichier), path_cible, true);
            return;
        }
        else
        {
            try
            {
                File.Copy(path, path_cible, true);
            }
            catch (Exception e)
            {
                Console.WriteLine("Il y'a eu une erreur, veuillez vérifier votre chemin");
            }
        }
    }
}

