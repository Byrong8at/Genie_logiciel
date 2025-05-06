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

                    language_choice(saver);

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

        Console.WriteLine("1. Choisir la langue/choice your language");

        Console.WriteLine("2. Choisir la sauvegarde");

        Console.WriteLine("3. Enregistrement");

        Console.WriteLine("7. Quitter");

        Console.WriteLine("=============================");

        Console.Write("Votre choix : ");

    }

    static void language_choice(saver saver)
    {

        Console.Write("Choissisez la langue souhaitez (fr/en): ");

        string titre = Console.ReadLine();
        //configurer language et changer si c en
        Livre nouveauLivre = new Livre(titre, auteur);

        saver.save_path(nouveauLivre);

    }

    static void save_selection(saver saver)

    {

        Console.Write("Choissisez la ou les sauvegardes souhaitez: ");

        string sauvegarde = Console.ReadLine();

        saver.open_save(sauvegarde);

    }

    static void path_saver(saver saver)

    {

        Console.Write("Choisir le numéro de sauvegarde souhaitez : ");

        string number = Console.ReadLine();

        Console.Write("Donner le chemin du dossier ou fichier que tu souhiates transferer : ");

        string path = Console.ReadLine();

        Console.Write("Donner le chemin du dossier de destination : ");

        string destination_path = Console.ReadLine();

        Console.Write("Choissisez le type de sauvegarde : ");

        string save_type = Console.ReadLine();

        Save_work nouveau_chemin = new Save_work(number, path, destination_path,save_type);

        Save_work Save_work = saver.save_path(path, destination_path);

    }

}



