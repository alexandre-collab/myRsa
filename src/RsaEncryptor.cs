using System;
using System.IO;
using System.Linq;

namespace src;
public class RsaEncryptor
{
    private readonly string publicKey;
    private readonly string textToEncrypt;

    public RsaEncryptor(string publicKey, string textToEncrypt)
    {
        this.publicKey = publicKey;
        this.textToEncrypt = textToEncrypt;
    }

    public void Encrypt(bool useOutputFilePath)
    {
        if (string.IsNullOrEmpty(publicKey) || string.IsNullOrEmpty(textToEncrypt))
        {
            Console.WriteLine("Erreur : Clé publique ou texte à chiffrer non spécifié.");
            return;
        }

        // Vérifiez qu’il commence par '---begin monRSA public key ---'
        if (!publicKey.StartsWith("---begin monRSA public key---"))
        {
            Console.WriteLine("Erreur : Clé publique invalide.");
            return;
        }

        /*         
          Lisez la ligne 2 et extrayez-en les valeurs n et e :
        ◦ Base64_decode de l’ensemble
        ◦ hexa_vers_décimal de la partie avant le retour chariot => n
        ◦ hexa_vers_décimal de la partie après le retour chariot => e 
        */

        string[] publicKeyLines = publicKey.Split("\n");
        byte[] data = Convert.FromBase64String(publicKeyLines[1]);
        string publicKeyDecoded = System.Text.Encoding.UTF8.GetString(data);

        string[] publicKeyDecodedLines = publicKeyDecoded.Split("\n");

        ulong n = Convert.ToUInt64(publicKeyDecodedLines[0], 16);
        ulong e = Convert.ToUInt64(publicKeyDecodedLines[1], 16);

        /* Pour chiffrer une chaîne de caractères avec la clé publique n et e, il faut :
        • Transformer chaque caractère en entrée en un chiffre en utilisant le code ASCII
        • Assembler & redécouper cette chaîne en blocs :
        ◦ On prends des blocs de longueur (taille de n) – 1 de long
        ◦ On part de la droite et on complète, éventuellement le dernier avec des 0 non
        significatifs
        ◦ Chaque bloc en clair B est chiffré en un bloc C par la formule C = B exposant e modulo n
        • Assembler les blocs chiffrés C en une suite de chiffres
        • Transformer cette suite de chiffres en texte affichable grâce un double encodage ASCII puis
        Base64 
        */

        string encryptedText = "";

        foreach (char c in textToEncrypt)
        {
            int asciiCode = Convert.ToInt32(c);
            string asciiCodeString = asciiCode.ToString();
            encryptedText += asciiCodeString;
        }

        int blockSize = (int)(n.ToString().Length - 1);

        Console.WriteLine($"encryptedText: {encryptedText}");
        Console.WriteLine($"n: {(n.ToString().Length - 1)}");

        string blockExpanded = "";

        // ◦ On prend des blocs de longueur (taille de n) – 1 de long
        for (int i = 0; i < encryptedText.Length; i += blockSize)
        {
            int endIndex = Math.Min(i + blockSize, encryptedText.Length);
            string block = encryptedText.Substring(i, endIndex - i);

            // ◦ On part de la droite et on complète, éventuellement le dernier avec des 0 non significatifs
            int nbZeroToAdd = blockSize - block.Length;
            while (nbZeroToAdd > 0)
            {
                // add "0" to the left of the block concatened
                blockExpanded = "0" + blockExpanded;
                nbZeroToAdd--;
            }


            blockExpanded += block;
        }


        string encryptedTextBlocks = "";

        // ◦ On prend des blocs de longueur (taille de n) – 1 de long
        for (int i = 0; i < blockExpanded.Length; i += blockSize)
        {
            int endIndex = Math.Min(i + blockSize, blockExpanded.Length);
            string block = blockExpanded.Substring(i, endIndex - i);

            ulong blockInt = Convert.ToUInt64(block);
            // ◦ Chaque bloc en clair B est chiffré en un bloc C par la formule C = B exposant e modulo n
            // PS : Performant sur mon pc (instantané), 16 go de ram + c#. Je ne sais pas si c'est le cas sur tous les pc.
            ulong encryptedBlockInt = MathTool.ModuloExponentiation(blockInt, e, n);

            // • Assembler les blocs chiffrés C en une suite de chiffres
            encryptedTextBlocks += encryptedBlockInt.ToString();

        }

        // Console.WriteLine($"encryptedTextBlocks: {encryptedTextBlocks}");

        // • Transformer cette suite de chiffres en texte affichable grâce un double encodage ASCII puis Base64 

        // Convert string to byte[]
        byte[] encryptedTextBytes = System.Text.Encoding.UTF8.GetBytes(encryptedTextBlocks);

        foreach (byte b in encryptedTextBytes)
        {
            Console.WriteLine(b);
        }
        // Convert byte[] to Base64 String
        string encryptedTextBase64 = Convert.ToBase64String(encryptedTextBytes);

        if(useOutputFilePath)
        {
            string outputFilePath = "encrypted.txt";
            File.WriteAllText(outputFilePath, encryptedTextBase64);
        }
        else
        {
            Console.WriteLine($"Texte chiffré : {encryptedTextBase64}");
        }
    }

}