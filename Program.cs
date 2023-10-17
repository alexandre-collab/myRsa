using System;
using src;
class Program
{
    static void Main(string[] args)
    {

        if (args.Length == 0 || args[0].ToLower() == "help")
        {
            displayHelp();
            return;
        }

        string module = args[0].ToLower();

        switch (module)
        {
            case "keygen":
                if (args.Length > 1 && !string.IsNullOrEmpty(args[1]))
                {
                    string key = args[1];
                    int size = 10;

                    for (int i = 2; i < args.Length; i++)
                    {
                        switch (args[i])
                        {
                            // size is optional, define by -s <size>
                            case "-s":
                                if (i + 1 < args.Length && int.TryParse(args[i + 1], out int parsedSize))
                                {
                                    size = parsedSize;
                                    i++;
                                }
                                else
                                {
                                    Console.WriteLine("Erreur de parsing de la taille.");
                                    return;
                                }
                                break;

                            default:
                                Console.WriteLine($"Argument {args[i]} inconnu.");
                                break;
                        }
                    }

                    Keygen keygen = new Keygen(key, size: size);
                    keygen.GenerateKey();

                    Console.WriteLine("Key generated successfully.");
                }
                else
                {
                    Console.WriteLine("Please provide a valid key as an argument for keygen.");
                }
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
