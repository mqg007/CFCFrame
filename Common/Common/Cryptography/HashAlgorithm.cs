using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Common.Cryptography
{
    public sealed partial class HashAlgorithm
    {
        public static byte[] ComputeHash(byte[] input, string algorithm)
        {
            if (input == null)
                throw new ArgumentNullException();

            System.Security.Cryptography.HashAlgorithm hash;
            switch (algorithm.ToUpperInvariant())
            {
                case "BKDR":
                    hash = new BKDRManaged();
                    break;
                case "MD5":
                    hash = MD5CryptoServiceProvider.Create();
                    break;
                case "MD160":
                case "RIPEMD160":
                    hash = RIPEMD160Managed.Create();
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
            byte[] result = hash.ComputeHash(input);
            hash.Clear();
            return result;
        }

        public static byte[] ComputeHash(System.IO.Stream input, string algorithm)
        {
            if (input == null || !input.CanRead)
                throw new ArgumentException();

            System.Security.Cryptography.HashAlgorithm hash;
            switch (algorithm.ToUpperInvariant())
            {
                case "BKDR":
                    hash = new BKDRManaged();
                    break;
                case "MD5":
                    hash = MD5CryptoServiceProvider.Create();
                    break;
                case "MD160":
                case "RIPEMD160":
                    hash = RIPEMD160Managed.Create();
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
            byte[] result = hash.ComputeHash(input);
            hash.Clear();
            return result;
        }


        public static byte[] ComputeHash(byte[] input)
        {
            return ComputeHash(input, "SHA1");
        }

        public static string ComputeHash(string input)
        {
            return ComputeHashBase64(input, "SHA1", Encoding.UTF8);
        }

        public static string ComputeHash(FileInfo file)
        {
            return ComputeHashBase64(file, "SHA1");
        }

        public static string ComputeHashBase64(string input)
        {
            return ComputeHashBase64(input, "SHA1", Encoding.UTF8);
        }

        public static string ComputeHashBase64(string input, string algorithm, Encoding encoding)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException();

            byte[] buffer = ComputeHash(encoding.GetBytes(input), algorithm);
            return Convert.ToBase64String(buffer);
        }

        public static string ComputeHashBase64(FileInfo file, string algorithm)
        {
            using (FileStream stream = file.OpenRead())
            {
                return ComputeHashBase64(stream, algorithm);
            }
        }

        public static string ComputeHashBase64(Stream stream, string algorithm)
        {
            if (stream == null || !stream.CanRead)
                throw new ArgumentNullException();

            byte[] buffer = ComputeHash(stream, algorithm);
            return Convert.ToBase64String(buffer);
        }

        public static string ComputeHashHex(string input)
        {
            return ComputeHashHex(input, "SHA1", Encoding.UTF8);
        }

        public static string ComputeHashHex(string input, string algorithm, Encoding encoding)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException("input");

            byte[] buffer = ComputeHash(encoding.GetBytes(input), algorithm);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
                result.Append(buffer[i].ToString("x2"));
            return result.ToString();
        }

        public static string ComputeHashHex(FileInfo file, string algorithm)
        {
            using (FileStream stream = file.OpenRead())
            {
                return ComputeHashHex(stream, algorithm);
            }
        }

        public static string ComputeHashHex(Stream stream, string algorithm)
        {
            if (stream == null || !stream.CanRead)
                throw new ArgumentNullException();

            byte[] buffer = ComputeHash(stream, algorithm);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
                result.Append(buffer[i].ToString("x2"));
            return result.ToString();
        }
    }
}