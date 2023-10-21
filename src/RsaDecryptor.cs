using System;
using System.IO;
using System.Numerics;

namespace src;
public class RsaDecryptor
{
    private readonly string privateKey;
    private readonly string textToDecrypt;

    public RsaDecryptor(string privateKey, string textToDecrypt)
    {
        this.privateKey = privateKey;
        this.textToDecrypt = textToDecrypt;
    }

    /// <summary>
    /// Decrypts the text using the private key.
    /// </summary>
    /// <param name="useOutputFile">If true, the decrypted text will be written to a file named 'decrypted.txt'.</param>
    public void Decrypt(bool useOutputFile)
    {
        if (string.IsNullOrEmpty(privateKey) || string.IsNullOrEmpty(textToDecrypt))
        {
            Console.WriteLine("Erreur : Clé privée ou texte chiffré non spécifié.");
            return;
        }

        // Vérifiez qu’il commence par '---begin monRSA private key ---'
        if (!privateKey.StartsWith("---begin monRSA private key---"))
        {
            Console.WriteLine("Erreur : Clé privée invalide.");
            return;
        }

        /* Lisez la ligne 2 et extrayez-en les valeurs n et d :
        ◦ Base64_decode de l’ensemble
        ◦ hexa_vers_décimal de la partie avant le retour chariot => n
        ◦ hexa_vers_décimal de la partie après le retour chariot => d */

        string[] privateKeyLines = privateKey.Split("\n");
        byte[] data = Convert.FromBase64String(privateKeyLines[1]);
        string privateKeyDecoded = System.Text.Encoding.UTF8.GetString(data);

        string[] privateKeyDecodedLines = privateKeyDecoded.Split("\n");

        string nString = privateKeyDecodedLines[0];
        string dString = privateKeyDecodedLines[1];

        // Remove "0x" prefix if it exists
        nString = nString.Replace("0x", "");
        dString = dString.Replace("0x", "");

        // convert hexa to decimal

        BigInteger n = 0;
        BigInteger d = 0;
        try
        {
            n = BigInteger.Parse(nString, System.Globalization.NumberStyles.HexNumber);
            d = BigInteger.Parse(dString, System.Globalization.NumberStyles.HexNumber);
        }
        catch (Exception)
        {
            Console.WriteLine("Erreur : Clé privée invalide. Vérifiez que la clé privée est bien au format hexadécimal.");
            return;
        }

        /* « défaire » les 2 encodages de la dernière étape de chiffrement : Base64 décode puis ASCII encode
        • Découper la chaîne en blocs C de longueur (taille de n) – 1 de long
        • Déchiffrer chaque bloc C en un bloc B avec la formule B = C exposant d modulo n
        • Découper cette nouvelle chaîne en blocs de 3 chiffes et procéder à un encodage ASCII */

        string textToDecryptDecoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(textToDecrypt));
        string decryptedTextBlockExpended = "";

        int blockSize = n.ToString().Length;

        for (int i = 0; i < textToDecryptDecoded.Length; i += blockSize)
        {
            // Découper la chaîne en blocs C de longueur (taille de n) – 1 de long
            int intervalMin = i;
            int intervalMax = Math.Min(blockSize, textToDecryptDecoded.Length - i);

            string block = textToDecryptDecoded.Substring(intervalMin, intervalMax);

            BigInteger blockInt = BigInteger.Parse(block);

            // Déchiffrer chaque bloc C en un bloc B avec la formule B = C exposant d modulo n
            BigInteger b = MathTool.ModuloExponentiation(blockInt, d, n);

            // In case the start of b should be a 0 but the integer convertion erase it, we add it
            if (b.ToString().Length % 3 > 0)
            {
                decryptedTextBlockExpended += "0" + b.ToString();
            }
            else
            {
                decryptedTextBlockExpended += b.ToString();
            }
        }

        string decryptedText = "";

        // Découper cette nouvelle chaîne en blocs de 3 chiffres et procéder à un encodage ASCII
        for (int i = 0; i < decryptedTextBlockExpended.Length; i += 3)
        {
            string block = decryptedTextBlockExpended.Substring(i, Math.Min(3, decryptedTextBlockExpended.Length - i));

            if (block.Length == 2)
            {
                block = "0" + block;
            }

            int asciiCode = Convert.ToInt32(block);
            decryptedText += (char)asciiCode;
        }

        if (useOutputFile)
        {
            File.WriteAllText("decrypted.txt", decryptedText);
            Console.WriteLine("Texte déchiffré dans le fichier : decrypted.txt");
        }
        else
        {
            Console.WriteLine(decryptedText);
        }

    }
}