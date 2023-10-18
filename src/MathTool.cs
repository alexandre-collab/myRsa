using System;

namespace src;

class MathTool
{
    /// <summary>
    /// Generates a random prime number with a specified number of digits.
    /// </summary>
    /// <param name="size">The number of digits in the prime number.</param>
    /// <returns>A random prime number.</returns>
    public static ulong GenerateRandomPrime(int size)
    {
        ulong randomNumber = 0;

        while (!IsPrime(randomNumber))
        {
            randomNumber = (ulong)new Random().Next((int)Math.Pow(10, size - 1), (int)Math.Pow(10, size) - 1);
        }

        return randomNumber;
    }

    /// <summary>
    /// Checks if a given number is a prime number.
    /// </summary>
    /// <param name="number">The number to check for primality.</param>
    /// <returns>True if the number is prime, false otherwise.</returns>
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

    /// <summary>
    /// Calculates the modular inverse of a number 'e' modulo 'm'.
    /// </summary>
    /// <param name="e">The number for which to find the modular inverse.</param>
    /// <param name="m">The modulo value.</param>
    /// <returns>The modular inverse of 'e' modulo 'm'.</returns>
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

    /// <summary>
    /// Calculates the greatest common divisor (GCD) of two numbers.
    /// </summary>
    /// <param name="a">The first number.</param>
    /// <param name="b">The second number.</param>
    /// <returns>The GCD of 'a' and 'b'.</returns>
    public static ulong Pgcd(ulong a, ulong b)
    {
        ulong r = 0;

        // Use Euclidean Algorithm to find GCD
        while (b != 0)
        {
            r = a % b;
            a = b;
            b = r;
        }

        return a;
    }

    internal static ulong ModuloExponentiation(ulong v, ulong e, ulong n)
    {
        ulong result = 1;

        while (e > 0)
        {
            if ((e & 1) == 1)
            {
                result = (result * v) % n;
            }

            e >>= 1;
            v = (v * v) % n;
        }

        return result;
    }
}