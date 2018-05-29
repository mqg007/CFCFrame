using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Cryptography
{
    public sealed partial class SaltedHashAlgorithm
    {
        public static byte[] ComputeHash(byte[] input, byte[] salt, string algorithm)
        {
            if (input == null || input.Length == 0)
                throw new ArgumentException();

            if (salt == null || salt.Length == 0)
                salt = RandomGenerator.GetBytes(8);

            System.Security.Cryptography.HashAlgorithm hash;
            switch (algorithm.ToUpper())
            {
                case "MD5":
                    hash = MD5CryptoServiceProvider.Create();
                    break;
                case "SHA":
                case "SHA1":
                    hash = SHA1CryptoServiceProvider.Create();
                    break;
                case "SHA256":
                    hash = SHA256Managed.Create();
                    break;
                case "SHA384":
                    hash = SHA384Managed.Create();
                    break;
                case "SHA512":
                    hash = SHA512Managed.Create();
                    break;
                default:
                    throw new NotSupportedException();
            }
            byte[] buffer = new byte[input.Length + salt.Length];
            Array.Copy(input, 0, buffer, 0, input.Length);
            Array.Copy(salt, 0, buffer, input.Length, salt.Length);
            byte[] result = hash.ComputeHash(buffer);
            return result;
            buffer = new byte[result.Length + salt.Length];
            Array.Copy(result, 0, buffer, 0, result.Length);
            Array.Copy(salt, 0, buffer, result.Length, salt.Length);
            hash.Clear();
            return buffer;
        }

        public static bool VerifyHash(byte[] input, byte[] value, string algorithm)
        {
            int hashLength;
            switch (algorithm.ToUpper())
            {
                case "MD5":
                    hashLength = 16;
                    break;
                case "SHA":
                case "SHA1":
                    hashLength = 20;
                    break;
                case "SHA256":
                    hashLength = 32;
                    break;
                case "SHA384":
                    hashLength = 48;
                    break;
                case "SHA512":
                    hashLength = 64;
                    break;
                default:
                    throw new NotSupportedException();
            }
            byte[] hash = new byte[hashLength];
            byte[] salt = new byte[value.Length - hashLength];
            Array.Copy(value, 0, hash, 0, hashLength);
            Array.Copy(value, hashLength, salt, 0, salt.Length);
            byte[] verifyHash = ComputeHash(input, salt, algorithm);
            return string.CompareOrdinal(Convert.ToBase64String(value), Convert.ToBase64String(verifyHash)) == 0;
        }


        public static byte[] ComputeHash(byte[] input, string algorithm)
        {
            return ComputeHash(input, null, algorithm);
        }

        public static string ComputeHash(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException();

            byte[] result = ComputeHash(Encoding.UTF8.GetBytes(input), "SHA1");
            return Convert.ToBase64String(result);
        }

        public static bool VerifyHash(string input, string value)
        {
            return VerifyHash(Encoding.UTF8.GetBytes(input), Convert.FromBase64String(value), "SHA1");
        }
    }
}