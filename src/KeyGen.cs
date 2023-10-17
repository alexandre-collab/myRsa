using System;
using System.IO;

namespace src;

class Keygen
{
    const string PUBLIC_KEY = "public";
    const string PRIVATE_KEY = "private";

    private readonly string key;
    private readonly string fileName;
    private readonly int size;
    private ulong p, q, n, nPrime, d, e = 0;


    /// <summary>
    /// Constructor for the Keygen class.
    /// </summary>
    /// <param name="key">The key to generate.</param>
    /// <param name="fileName">File name (optional, default is "monRSA").</param>
    /// <param name="size">The size of the key (optional, default is 10).</param>
    public Keygen(string key, string fileName = "monRSA", int size = 10)
    {
        this.key = key;
        this.fileName = fileName;
        this.size = size;
    }

    public void GenerateKey()
    {
        this.Calculate();
        this.GeneratePublicKey();
        this.GeneratePrivateKey();
    }

    private void Calculate()
    {
        // Générer 2 nombres premier différents de 10 chiffres de long: p & q
        this.p = MathTool.GenerateRandomPrime(this.size);
        while (this.q == 0 || this.q == this.p)
        {
            this.q = MathTool.GenerateRandomPrime(this.size);
        }

        // Calculer n = p * q
        this.n = this.p * this.q;

        // Calculer n' = (p - 1) * (q - 1)
        this.nPrime = (this.p - 1) * (this.q - 1);

        //  Faire une boucle
        //  qui teste des valeurs pour e et d jusqu’à ce que :
        // ◦ e soit premier
        // ◦ e soit différent de d
        // ◦ ed % n’ = 1 (ou, dit autrement, ed = 1 modulo n’)

        // Trouver e tel que (e * d) % n' == 1

        this.e = MathTool.GenerateRandomPrime(this.size);

        while (MathTool.Pgcd(this.e, this.nPrime) != 1 || this.e == this.d)
        {
            this.e = MathTool.GenerateRandomPrime(this.size);
        }

        this.d = MathTool.ModuloInverse(this.e, this.nPrime);

    }

    private void GeneratePublicKey()
    {
        string contentPublicFile = WriteStartingLine(PUBLIC_KEY);
        contentPublicFile += "\n";

        // base64_encode(decimal_vers_hexa(n), retour chariot, decimal_vers_hexa(e))

        contentPublicFile += Convert.ToBase64String(BitConverter.GetBytes(this.n));
        contentPublicFile += "\n";
        contentPublicFile += Convert.ToBase64String(BitConverter.GetBytes(this.e));

        contentPublicFile += "\n";
        contentPublicFile += WriteEndingLine(PUBLIC_KEY);

        // save file in folder project
        File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{this.fileName}.pub"), contentPublicFile);

        return;
    }

    private void GeneratePrivateKey()
    {
        string contentPrivateFile = WriteStartingLine(PRIVATE_KEY);
        contentPrivateFile += "\n";

        // base64_encode(decimal_vers_hexa(n), retour chariot, decimal_vers_hexa(d))

        contentPrivateFile += Convert.ToBase64String(BitConverter.GetBytes(this.n));
        contentPrivateFile += "\n";
        contentPrivateFile += Convert.ToBase64String(BitConverter.GetBytes(this.d));

        contentPrivateFile += "\n";
        contentPrivateFile += WriteEndingLine(PRIVATE_KEY);

        // save file in folder project
        File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{this.fileName}.priv"), contentPrivateFile);

        return;
    }

    private static string WriteStartingLine(string keyType)
    {

        if (keyType == PUBLIC_KEY)
        {
            return $"---begin monRSA {PUBLIC_KEY} key---";
        }
        else if (keyType == PRIVATE_KEY)
        {
            return $"---begin monRSA {PRIVATE_KEY} key---";
        }
        else
        {
            throw new System.ArgumentException("Key type must be 'public' or 'private'.");
        }
    }

    private static string WriteEndingLine(string keyType)
    {

        if (keyType == PUBLIC_KEY)
        {
            return $"---end monRSA {PUBLIC_KEY} key---";
        }
        else if (keyType == PRIVATE_KEY)
        {
            return $"---end monRSA {PRIVATE_KEY} key---";
        }
        else
        {
            throw new System.ArgumentException("Key type must be 'public' or 'private'.");
        }
    }



}