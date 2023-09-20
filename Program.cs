using System;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            Console.WriteLine("Utilisation : program.exe \"Texte à chiffrer\"");
            return;
        }

        string plaintext = args[0];

        // Reste du code pour générer les clés et chiffrer le texte
    }
}
