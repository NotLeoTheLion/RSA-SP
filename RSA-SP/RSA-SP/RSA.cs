using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace RSA_SP
{
    internal static class RSA
    {
        private static BigInteger GeneratePrime(int bits)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[bits / 8];
            BigInteger prime;

            do
            {
                rng.GetBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F;
                prime = new BigInteger(bytes);

                // Ensure it's odd
                if (prime % 2 == 0)
                    prime += 1;

            } while (!IsPrime(prime, 10));

            return prime;
        }

        private static bool IsPrime(BigInteger number, int iterations)
        {
            if (number == 2 || number == 3)
                return true;
            if (number < 2 || number % 2 == 0)
                return false;

            BigInteger d = number - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            for (int i = 0; i < iterations; i++)
            {
                BigInteger a = RandomIntegerBelow(number - 2) + 2;
                BigInteger x = BigInteger.ModPow(a, d, number);
                if (x == 1 || x == number - 1)
                    continue;

                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, number);
                    if (x == 1)
                        return false;
                    if (x == number - 1)
                        break;
                }

                if (x != number - 1)
                    return false;
            }

            return true;
        }

        private static BigInteger RandomIntegerBelow(BigInteger n)
        {
            byte[] bytes = n.ToByteArray();
            BigInteger r;

            do
            {
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                rng.GetBytes(bytes);
                bytes[bytes.Length - 1] &= (byte)0x7F;
                r = new BigInteger(bytes);
            } while (r >= n);

            return r;
        }

        public static (BigInteger publicKey, BigInteger privateKey, BigInteger modulus) GenerateKeys(int bits)
        {
            BigInteger p = GeneratePrime(bits);
            BigInteger q = GeneratePrime(bits);
            BigInteger n = p * q;
            BigInteger phi = (p - 1) * (q - 1);
            BigInteger e = FindCoprime(phi);
            BigInteger d = ModInverse(e, phi);
            return (e, d, n);
        }

        private static BigInteger FindCoprime(BigInteger phi)
        {
            BigInteger e = 3;
            while (BigInteger.GreatestCommonDivisor(e, phi) != 1)
            {
                e += 2;
            }
            return e;
        }

        public static string Encrypt(string message, BigInteger publicKey, BigInteger modulus)
        {
            StringBuilder encryptedMessage = new StringBuilder();

            foreach (char character in message)
            {
                BigInteger msgNumeric = new BigInteger(Encoding.UTF8.GetBytes(new[] { character }));
                BigInteger encryptedChar = BigInteger.ModPow(msgNumeric, publicKey, modulus);
                encryptedMessage.Append(encryptedChar + " ");
            }

            return encryptedMessage.ToString().Trim();
        }

        private static BigInteger ModInverse(BigInteger e, BigInteger phi)
        {
            BigInteger x, y;
            BigInteger g = ExtendedGCD(e, phi, out x, out y);
            if (g != 1)
                throw new ArgumentException("");
            return (x % phi + phi) % phi;
        }

        private static BigInteger ExtendedGCD(BigInteger a, BigInteger b, out BigInteger x, out BigInteger y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }
            BigInteger x1, y1;
            BigInteger gcd = ExtendedGCD(b % a, a, out x1, out y1);
            x = y1 - (b / a) * x1;
            y = x1;
            return gcd;
        }
    }
}
