using System;
using System.IO;
using src;
class Program
{
    static void Main(string[] args)
    {

        if (args.Length == 0 || args[0].ToLower() == "help")
        {
            DisplayHelp();
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
                    string fileName = "monRSA";

                    int currentArgumentIndex = 2;

                    for (int i = currentArgumentIndex; i < args.Length; i++)
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
                            case "-f":
                                if (i + 1 < args.Length && !string.IsNullOrEmpty(args[i + 1]))
                                {
                                    fileName = args[i + 1];
                                    i++;
                                }
                                else
                                {
                                    Console.WriteLine("Erreur : Nom de fichier non spécifié.");
                                    return;
                                }
                                break;

                            default:
                                Console.WriteLine($"Argument {args[i]} inconnu.");
                                break;
                        }
                    }

                    Keygen keygen = new Keygen(key, fileName: fileName, size: size);
                    keygen.GenerateKey();

                    Console.WriteLine("Clés générées avec succès.");
                }
                else
                {
                    Console.WriteLine("Veuillez spécifier la clé.");
                }
                break;
            case "crypt":
                if (args.Length > 1 && !string.IsNullOrEmpty(args[1]) && !string.IsNullOrEmpty(args[2]))
                {
                    string publicKey = args[1];
                    string textToEncrypt = args[2];
                    bool useInputFile = false;
                    bool useOutputFile = false;

                    int currentArgumentIndex = 3;

                    for (int i = currentArgumentIndex; i < args.Length; i++)
                    {
                        switch (args[i])
                        {
                            // input file is optional, define by -i <input file>
                            case "-i":
                                useInputFile = true;
                                break;

                            // output file is optional, define by -o <output file>
                            case "-o":
                                useOutputFile = true;
                                break;

                            default:
                                Console.WriteLine($"Argument {args[i]} inconnu.");
                                break;
                        }
                    }

                    if (useInputFile)
                    {
                        if (!File.Exists(textToEncrypt))
                        {
                            Console.WriteLine($"Le fichier {textToEncrypt} n'existe pas.");
                            return;
                        }

                        if (!File.Exists(publicKey))
                        {
                            Console.WriteLine($"Le fichier {publicKey} n'existe pas.");
                            return;
                        }

                        publicKey = File.ReadAllText(publicKey);
                        textToEncrypt = File.ReadAllText(textToEncrypt);
                    }

                    RsaEncryptor rsaEncryptor = new RsaEncryptor(publicKey, textToEncrypt);
                    rsaEncryptor.Encrypt(useOutputFile);
                }
                else
                {
                    Console.WriteLine("Des paramètres sont manquants.");
                }

                break;
            case "decrypt":
                if (args.Length > 1 && !string.IsNullOrEmpty(args[1]) && !string.IsNullOrEmpty(args[2]))
                {
                    string privateKey = args[1];
                    string textToDecrypt = args[2];
                    bool useInputFile = false;
                    bool useOutputFile = false;

                    int currentArgumentIndex = 3;

                    for (int i = currentArgumentIndex; i < args.Length; i++)
                    {
                        switch (args[i])
                        {
                            // input file is optional, define by -i <input file>
                            case "-i":
                                useInputFile = true;
                                break;

                            // output file is optional, define by -o <output file>
                            case "-o":
                                useOutputFile = true;
                                break;

                            default:
                                Console.WriteLine($"Argument {args[i]} inconnu.");
                                break;
                        }
                    }

                    if (useInputFile)
                    {
                        if (!File.Exists(textToDecrypt))
                        {
                            Console.WriteLine($"Le fichier {textToDecrypt} n'existe pas.");
                            return;
                        }

                        if (!File.Exists(privateKey))
                        {
                            Console.WriteLine($"Le fichier {privateKey} n'existe pas.");
                            return;
                        }

                        privateKey = File.ReadAllText(privateKey);
                        textToDecrypt = File.ReadAllText(textToDecrypt);
                    }

                    RsaDecryptor rsaDecryptor = new RsaDecryptor(privateKey, textToDecrypt);
                    rsaDecryptor.Decrypt(useOutputFile);
                }
                else
                {
                    Console.WriteLine("Des paramètres sont manquants.");
                }

                break;
            default:
                Console.WriteLine("Erreur : Commande invalide. Utilisez 'help' pour afficher le manuel.");
                break;
        }
    }

    // Display the help
    static void DisplayHelp()
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
