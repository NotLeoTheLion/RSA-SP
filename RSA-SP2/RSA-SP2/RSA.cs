using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace RSA_SP2
{
    internal static class RSA
    {
        public static bool ValidateSignature(string originalMessage, string signature, BigInteger publicKey, BigInteger modulus)
        {
            var encryptedValues = signature.Split(' ');
            var originalBytes = Encoding.UTF8.GetBytes(originalMessage);

            if (originalBytes.Length != encryptedValues.Length)
            {
                return false;
            }

            for (int i = 0; i < originalBytes.Length; i++)
            {
                if (!BigInteger.TryParse(encryptedValues[i], out BigInteger encryptedValue))
                {
                    return false;
                }

                BigInteger decryptedValue = BigInteger.ModPow(encryptedValue, publicKey, modulus);

                byte decryptedChar = (byte)(decryptedValue % 256); 

                if (originalBytes[i] != decryptedChar)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
