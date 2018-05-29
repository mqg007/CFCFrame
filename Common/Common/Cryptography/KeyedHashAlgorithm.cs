using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Common.Cryptography
{
    public sealed partial class KeyedHashAlgorithm
    {
        public static byte[] ComputeHash(byte[] input, byte[] key, string algorithm)
        {
            if (input == null || input.Length == 0)
                throw new ArgumentException();
            if (key == null || key.Length == 0)
                throw new ArgumentException();

            System.Security.Cryptography.KeyedHashAlgorithm hash;
            switch (algorithm.ToUpperInvariant())
            {
                case "MD5":
                case "HMACMD5":
                    hash = HMACMD5.Create();
                    break;
                case "MD160":
                case "RIPEMD160":
                case "HMACRIPEMD160":
                    hash = HMACRIPEMD160.Create();
                    break;
                case "SHA":
                case "SHA1":
                case "HMACSHA":
                case "HMACSHA1":
                    hash = HMACSHA1.Create();
                    break;
                case "SHA256":
                case "HMACSHA256":
                    hash = HMACSHA256.Create();
                    break;
                case "SHA384":
                case "HMACSHA384":
                    hash = HMACSHA384.Create();
                    break;
                case "SHA512":
                case "HMACSHA512":
                    hash = HMACSHA512.Create();
                    break;
                default:
                    throw new NotSupportedException();
            }
            hash.Key = key;
            byte[] result = hash.ComputeHash(input);
            hash.Clear();
            return result;
        }

        public static byte[] ComputeHash(byte[] input, byte[] key)
        {
            return ComputeHash(input, key, "HMACSHA1");
        }

        public static string ComputeHash(string input, string key)
        {
            return ComputeHash(input, key, "HMACSHA1", Encoding.UTF8);
        }

        public static string ComputeHash(string input, string key, string algorithm, Encoding encoding)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException();
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException();

            byte[] skey = encoding.GetByteCount(key) > 64 ? HashAlgorithm.ComputeHash(encoding.GetBytes(key)) : encoding.GetBytes(key);
            byte[] result = ComputeHash(encoding.GetBytes(input), skey, algorithm);
            return Convert.ToBase64String(result);
        }
    }
}