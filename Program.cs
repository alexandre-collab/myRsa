using System;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0 || args[0].ToLower() == "help")
        {
            displayHelp();
            return;
        }

        string commande = args[0].ToLower();

        switch (commande)
        {
            case "keygen":
                Console.WriteLine("keygen");
                break;
            case "crypt":
                Console.WriteLine("crypt");
                break;
            case "decrypt":
                Console.WriteLine("decrypt");
                break;
            default:
                Console.WriteLine("Erreur : Commande invalide. Utilisez 'help' pour afficher le manuel.");
                break;
        }
    }

        static void displayHelp()
    {
        Console.WriteLine("Manuel d'utilisation :");
        Console.WriteLine("Syntaxe : monRSA <commande> [<clé>] [<texte>] [switchs]");
        Console.WriteLine("Commande :");
        Console.WriteLine("keygen : Génère une paire de clé");
        Console.WriteLine("crypt : Chiffre <texte> pour le clé publique <clé>");
        Console.WriteLine("decrypt : Déchiffre <texte> pour le clé privée <clé>");
        Console.WriteLine("help : Affiche ce manuel");
        Console.WriteLine("Clé : Un fichier qui contient une clé publique monRSA ('crypt') ou une clé privée ('decrypt')");
        Console.WriteLine("Texte : Une phrase en clair ('crypt') ou une phrase chiffrée ('decrypt')");
        Console.WriteLine("Switchs : -f <file> permet de choisir le nom des clés générées, monRSA.pub et monRSA.priv par défaut");
        Console.WriteLine("Contrôlez que les paramètres sont corrects.");
    }
}
