using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common.Cryptography
{
    public sealed partial class SymmetricAlgorithm
    {
        public static byte[] Encrypt(byte[] input, byte[] key, byte[] salt, string algorithm)
        {
            if (input == null || input.Length < 1 ||
                key == null || key.Length < 1 ||
                salt == null || salt.Length < 1)
                throw new ArgumentException();

            System.Security.Cryptography.SymmetricAlgorithm sAlgorithm;
            switch (algorithm.ToUpperInvariant())
            {
                case "DES":
                    sAlgorithm = DESCryptoServiceProvider.Create();
                    break;
                case "3DES":
                case "TDES":
                case "TRIPLEDES":
                    sAlgorithm = TripleDESCryptoServiceProvider.Create();
                    break;
                case "AES":
                    sAlgorithm = AesCryptoServiceProvider.Create();
                    break;
                case "RC2":
                    sAlgorithm = RC2CryptoServiceProvider.Create();
                    break;
                case "RIJNDAEL":
                    sAlgorithm = new RijndaelManaged();
                    break;
                default:
                    throw new NotSupportedException();
            }
            DeriveBytes rdb = new Rfc2898DeriveBytes(key, salt, 1024);
            sAlgorithm.Key = rdb.GetBytes(sAlgorithm.KeySize / 8);
            sAlgorithm.IV = rdb.GetBytes(sAlgorithm.BlockSize / 8);
            byte[] reslut;
            using (ICryptoTransform encryptor = sAlgorithm.CreateEncryptor(sAlgorithm.Key, sAlgorithm.IV))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(input, 0, input.Length);
                        cryptoStream.FlushFinalBlock();
                        cryptoStream.Clear();
                    }
                    reslut = stream.ToArray();
                }
            }
            sAlgorithm.Clear();
            return reslut;
        }

        public static byte[] Decrypt(byte[] input, byte[] key, byte[] salt, string algorithm)
        {
            if (input == null || input.Length < 1 ||
                key == null || key.Length < 1 ||
                salt == null || salt.Length < 1)
                throw new ArgumentException();

            System.Security.Cryptography.SymmetricAlgorithm sAlgorithm;
            switch (algorithm.ToUpperInvariant())
            {
                case "DES":
                    sAlgorithm = DESCryptoServiceProvider.Create();
                    break;
                case "3DES":
                case "TDES":
                case "TRIPLEDES":
                    sAlgorithm = TripleDESCryptoServiceProvider.Create();
                    break;
                case "AES":
                    sAlgorithm = AesCryptoServiceProvider.Create();
                    break;
                case "RC2":
                    sAlgorithm = RC2CryptoServiceProvider.Create();
                    break;
                case "RIJNDAEL":
                    sAlgorithm = new RijndaelManaged();
                    break;
                default:
                    throw new NotSupportedException();
            }
            DeriveBytes rdb = new Rfc2898DeriveBytes(key, salt, 1024);
            sAlgorithm.Key = rdb.GetBytes(sAlgorithm.KeySize / 8);
            sAlgorithm.IV = rdb.GetBytes(sAlgorithm.BlockSize / 8);
            byte[] reslut;
            using (ICryptoTransform decryptor = sAlgorithm.CreateDecryptor(sAlgorithm.Key, sAlgorithm.IV))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(input, 0, input.Length);
                        cryptoStream.FlushFinalBlock();
                        cryptoStream.Clear();
                    }
                    reslut = stream.ToArray();
                }
            }
            sAlgorithm.Clear();
            return reslut;
        }

        public static byte[] Encrypt(byte[] input, byte[] key, byte[] salt)
        {
            return Encrypt(input, key, salt, "AES");
        }

        public static string Encrypt(string input, string key, string salt)
        {
            return Encrypt(input, key, salt, "AES", Encoding.UTF8);
        }

        public static string Encrypt(string input, string key, string salt, string algorithm, Encoding encoding)
        {
            return Convert.ToBase64String(Encrypt(encoding.GetBytes(input),
                Convert.FromBase64String(key),
                Convert.FromBase64String(salt),
                algorithm));
        }

        public static byte[] Decrypt(byte[] input, byte[] key, byte[] salt)
        {
            return Decrypt(input, key, salt, "AES");
        }

        public static string Decrypt(string input, string key, string salt)
        {
            return Decrypt(input, key, salt, "AES", Encoding.UTF8);
        }

        public static string Decrypt(string input, string key, string salt, string algorithm, Encoding encoding)
        {
            byte[] result = Decrypt(Convert.FromBase64String(input),
                Convert.FromBase64String(key),
                Convert.FromBase64String(salt),
                algorithm);
            return encoding.GetString(result, 0, result.Length);
        }
    }
}