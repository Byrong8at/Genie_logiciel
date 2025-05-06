using System;

class Program
{
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
                    break;

                case '2':
                    save_selection(saver);
                    break;

                case '3':
                    path_saver(saver);
                    break;

                case '7':
                    quitter = true;
                    Console.WriteLine("Merci d'avoir utilisé le gestionnaire de bibliothèque.");
                    break;

                default:
                    Console.WriteLine("Choix non valide, veuillez réessayer.");
                    break;
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    static void AfficherMenu()
    {
        Console.WriteLine("===== Menu Bibliothèque =====");
        Console.WriteLine("1. Choisir la langue / Choose your language");
        Console.WriteLine("2. Choisir la sauvegarde");
        Console.WriteLine("3. Lancer une sauvegarde");
        Console.WriteLine("7. Quitter");
        Console.WriteLine("=============================");
        Console.Write("Votre choix : ");
    }

    static void language_choice()
    {
        Console.Write("Choisissez la langue souhaitée (fr/en) : ");
        string langue = Console.ReadLine()?.ToLower();

        if (langue == "en")
            Console.WriteLine("Language set to English (not implemented).");
        else if (langue == "fr")
            Console.WriteLine("Langue définie sur le français.");
        else
            Console.WriteLine("Langue inconnue. Aucune modification.");
    }

    static void save_selection(saver saver)
    {
        Console.Write("Choisissez la ou les sauvegardes souhaitées (ex: 1-3 ou 1;3 ou 1) : ");
        string sauvegarde = Console.ReadLine();
        saver.open_save(sauvegarde);
    }

    static void path_saver(saver saver)
    {
        Console.Write("Numéro de sauvegarde : ");
        string number = Console.ReadLine();

        Console.Write("Chemin du fichier à sauvegarder : ");
        string sourcePath = Console.ReadLine();

        Console.Write("Chemin du dossier de destination : ");
        string targetPath = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(sourcePath) && !string.IsNullOrWhiteSpace(targetPath))
        {
            saver.save_path(number,sourcePath, targetPath);
        }
        else
        {
            Console.WriteLine("Les chemins ne peuvent pas être vides.");
        }
    }
}
