using System;
using System.IO;
using System.Numerics;

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


    /// <summary>
    /// Encrypts the text using the public key.
    /// </summary>
    /// <param name="useOutputFilePath">If true, the encrypted text will be written to a file named 'encrypted.txt'.</param>
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

        string nString = publicKeyDecodedLines[0];
        string eString = publicKeyDecodedLines[1];

        // Remove "0x" prefix if it exists
        nString = nString.Replace("0x", "");
        eString = eString.Replace("0x", "");

        // convert hexa to decimal
        BigInteger n = 0;
        BigInteger e = 0;

        try
        {
            n = BigInteger.Parse(nString, System.Globalization.NumberStyles.HexNumber);
            e = BigInteger.Parse(eString, System.Globalization.NumberStyles.HexNumber);
        }
        catch (Exception)
        {
            Console.WriteLine("Erreur : Clé publique invalide. Vérifiez que les valeurs n et e sont bien en hexadécimal.");
            return;
        }

        // /* Pour chiffrer une chaîne de caractères avec la clé publique n et e, il faut :
        // • Transformer chaque caractère en entrée en un chiffre en utilisant le code ASCII
        // • Assembler & redécouper cette chaîne en blocs :
        // ◦ On prends des blocs de longueur (taille de n) – 1 de long
        // ◦ On part de la droite et on complète, éventuellement le dernier avec des 0 non
        // significatifs
        // ◦ Chaque bloc en clair B est chiffré en un bloc C par la formule C = B exposant e modulo n
        // • Assembler les blocs chiffrés C en une suite de chiffres
        // • Transformer cette suite de chiffres en texte affichable grâce un double encodage ASCII puis
        // Base64 
        // */

        string encryptedText = "";

        foreach (char c in textToEncrypt)
        {
            // convert to ascii
            string asciiCodeString = ((int)c).ToString();

            // if on 2 digits, add a 0 at the beginning to have 3 digits blocks (ASCII)
            if (asciiCodeString.Length == 2)
            {
                asciiCodeString = "0" + asciiCodeString;
            }

            encryptedText += asciiCodeString;
        }

        int blockSize = (int)(n.ToString().Length - 1);
        int cBlockLength = blockSize + 1;

        string encryptedTextBlocks = "";

        // ◦ On prend des blocs de longueur (taille de n) – 1 de long
        for (int i = 0; i < encryptedText.Length - 1; i += blockSize)
        {
            // Découper la chaîne en blocs C de longueur (taille de n) – 1 de long
            int intervalMin = i;
            int intervalMax = Math.Min(blockSize, encryptedText.Length - i);

            string block = encryptedText.Substring(intervalMin, intervalMax);

            BigInteger blockInt = BigInteger.Parse(block);
            // ◦ Chaque bloc en clair B est chiffré en un bloc C par la formule C = B exposant e modulo n
            BigInteger encryptedBlockInt = MathTool.ModuloExponentiation(blockInt, e, n);

            // In case the start of b should be a or multiples 0 but the integer convertion erase it, we add them to have the right length of B block
            string zeroToCompleteLengthOfBlockC = "";
            for (int j = 0; j < cBlockLength - encryptedBlockInt.ToString().Length; j++)
            {
                zeroToCompleteLengthOfBlockC += "0";
            }

            // • Assembler les blocs chiffrés C en une suite de chiffres
            encryptedTextBlocks += zeroToCompleteLengthOfBlockC + encryptedBlockInt.ToString();
        }

        // • Transformer cette suite de chiffres en texte affichable grâce un double encodage ASCII puis Base64 

        // Convert string to byte[]
        byte[] encryptedTextBytes = System.Text.Encoding.UTF8.GetBytes(encryptedTextBlocks);

        // Convert byte[] to Base64 String
        string encryptedTextBase64 = Convert.ToBase64String(encryptedTextBytes);

        if (useOutputFilePath)
        {
            string outputFilePath = "encrypted.txt";
            File.WriteAllText(outputFilePath, encryptedTextBase64);
            Console.WriteLine($"Texte chiffré dans le fichier : {outputFilePath}");
        }
        else
        {
            Console.WriteLine($"Texte chiffré : {encryptedTextBase64}");
        }

    }

}