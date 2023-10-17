using System;

namespace src;

class MathTool
{
    public static ulong GenerateRandomPrime(int size)
    {
        ulong randomNumber = 0;

        while (!IsPrime(randomNumber))
        {
            randomNumber = (ulong)new Random().Next((int)Math.Pow(10, size - 1), (int)Math.Pow(10, size) - 1);
        }

        return randomNumber;
    }

    public static bool IsPrime(ulong number)
    {
        if (number < 2)
            return false;

        for (ulong i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0)
                return false;
        }

        return true;
    }

    public static ulong ModuloInverse(ulong e, ulong m)
    {
        ulong m0 = m;
        ulong y = 0, x = 1;

        if (m == 1)
            return 0;

        // Extended Euclidean Algorithm to find modular inverse
        while (e > 1)
        {
            ulong q = e / m;
            ulong t = m;

            m = e % m;
            e = t;
            t = y;

            y = x - q * y;
            x = t;
        }

        // Make x positive
        if (x < 0)
            x += m0;

        return x;
    }

    public static ulong Pgcd(ulong a, ulong b)
    {
        ulong r = 0;

        while (b != 0)
        {
            r = a % b;
            a = b;
            b = r;
        }

        return a;
    }

}