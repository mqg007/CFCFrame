using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Common.Cryptography
{
    public class RSACryptoAlgorithm
    {
        /// <summary>
        /// 获取所有读取的有效证书
        /// </summary>
        /// <returns></returns>
        public static X509Certificate2Collection GetCertificates()
        {
            X509Certificate2Collection reslut = null;
            X509Store store = null;
            try
            {
                for (int i = 1; i < 3; i++)
                {
                    for (int j = 1; j < 9; j++)
                    {
                        store = new X509Store((StoreName)j, (StoreLocation)i);
                        store.Open(OpenFlags.ReadOnly);
                        if (reslut == null)
                            reslut = store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                        else
                            try
                            {
                                reslut.AddRange(store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, false));
                            }
                            finally
                            {
                            }
                        store.Close();
                    }
                }
            }
            finally
            {
                if (store != null)
                    store.Close();
            }
            return reslut;
        }

        public static X509Certificate2 GetCertificateBySubjectName(string subject)
        {
            var result = GetCertificates();
            if (result == null || result.Count < 1)
                return null;
            X509Certificate2Collection certCollection = result
                .Find(X509FindType.FindByTimeValid, DateTime.Now, true)
                .Find(X509FindType.FindBySubjectName, subject, true);
            if (certCollection.Count == 0)
                return null;
            else
                return certCollection[0];
        }

        public static X509Certificate2 GetCertificateByThumbprint(string thumbprint)
        {
            var result = GetCertificates();
            if (result == null || result.Count < 1)
                throw new CryptographicException("不能读取指定证书");
                //return null;
            X509Certificate2Collection certCollection = result
                .Find(X509FindType.FindByTimeValid, DateTime.Now, true)
                .Find(X509FindType.FindByThumbprint, thumbprint, true);
            if (certCollection.Count == 0)
                throw new CryptographicException("不能读取指定证书");
                //return null;
            else
                return certCollection[0];
        }

        public static X509Certificate2 GetCertificateFromStore(string serialNumber, string thumbprint)
        {
            var result = GetCertificates();
            if (result == null || result.Count < 1)
                return null;
            X509Certificate2Collection certCollection = result
                    .Find(X509FindType.FindByTimeValid, DateTime.Now, true)
                    .Find(X509FindType.FindBySerialNumber, serialNumber, true)
                    .Find(X509FindType.FindByThumbprint, thumbprint, true);
            if (certCollection.Count == 0)
                return null;
            else
                return certCollection[0];
        }

        public static X509Certificate2 GetCertificateFromStore(StoreLocation storeLocation, string serialNumber, string thumbprint)
        {
            X509Store store = new X509Store(storeLocation);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = store.Certificates
                    .Find(X509FindType.FindByTimeValid, DateTime.Now, false)
                    .Find(X509FindType.FindBySerialNumber, serialNumber, false)
                    .Find(X509FindType.FindByThumbprint, thumbprint, false);
                if (certCollection.Count == 0)
                    return null;
                else
                    return certCollection[0];
            }
            finally
            {
                store.Close();
            }
        }

        public static X509Certificate2 GetCertificateFromFile(string filePath)
        {
            return X509Certificate2.CreateFromCertFile(filePath) as X509Certificate2;
        }

        public static byte[] Encrypt(byte[] data, string serialNumber, string thumbprint)
        {
            var cert = GetCertificateFromStore(serialNumber, thumbprint);
            if (cert == null)
                throw new CryptographicException("证书文件不存在");

            var cryptoServiceProvider = cert.PublicKey.Key;
            return Encrypt(data, cryptoServiceProvider);
        }

        public static byte[] Encrypt(byte[] data, StoreLocation storeLocation, string serialNumber, string thumbprint)
        {
            var cert = GetCertificateFromStore(storeLocation, serialNumber, thumbprint);
            if (cert == null)
                throw new CryptographicException("证书文件不存在");

            var cryptoServiceProvider = cert.PublicKey.Key;
            return Encrypt(data, cryptoServiceProvider);
        }

        public static byte[] Encrypt(byte[] data, AsymmetricAlgorithm rsaPublicKey)
        {
            byte[] result;
            var output = new MemoryStream();
            try
            {
                using (var input = new MemoryStream(data))
                {
                    EncryptData(output, input, rsaPublicKey);
                    result = output.ToArray();
                }
            }
            finally
            {
                output.Close();
            }
            return result;
        }

        private static void EncryptData(Stream output, Stream input, AsymmetricAlgorithm rsaPublicKey)
        {
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.KeySize = 256;
                aesManaged.BlockSize = 128;
                aesManaged.Mode = CipherMode.CBC;
                using (ICryptoTransform transform = aesManaged.CreateEncryptor())
                {
                    RSAPKCS1KeyExchangeFormatter keyFormatter = new RSAPKCS1KeyExchangeFormatter(rsaPublicKey);
                    byte[] keyEncrypted = keyFormatter.CreateKeyExchange(aesManaged.Key, aesManaged.GetType());
                    byte[] LenK = new byte[4];
                    byte[] LenIV = new byte[4];
                    int lKey = keyEncrypted.Length;
                    LenK = BitConverter.GetBytes(lKey);
                    int lIV = aesManaged.IV.Length;
                    LenIV = BitConverter.GetBytes(lIV);

                    output.Write(LenK, 0, 4);
                    output.Write(LenIV, 0, 4);
                    output.Write(keyEncrypted, 0, lKey);
                    output.Write(aesManaged.IV, 0, lIV);
                    using (CryptoStream streamEncrypted = new CryptoStream(output, transform, CryptoStreamMode.Write))
                    {
                        int count = 0;
                        int offset = 0;

                        int blockSizeBytes = aesManaged.BlockSize / 8;
                        byte[] buffer = new byte[blockSizeBytes];
                        int bytesRead = 0;

                        do
                        {
                            count = input.Read(buffer, 0, blockSizeBytes);
                            offset += count;
                            streamEncrypted.Write(buffer, 0, count);
                            bytesRead += blockSizeBytes;
                        }
                        while (count > 0);
                        input.Close();

                        streamEncrypted.FlushFinalBlock();
                        streamEncrypted.Close();
                    }
                }
            }
        }

        public static byte[] Decrypt(byte[] data, string thumbprint)
        {
            var cert = GetCertificateByThumbprint(thumbprint);
            if (cert == null)
                throw new CryptographicException("证书文件不存在");

            var cryptoServiceProvider = cert.PrivateKey as RSACryptoServiceProvider;
            return Decrypt(data, cryptoServiceProvider);
        }

        public static byte[] Decrypt(byte[] data, RSACryptoServiceProvider rsaPrivateKey)
        {
            byte[] result;
            var output = new MemoryStream();
            try
            {
                using (var input = new MemoryStream(data))
                {
                    DecryptData(output, input, rsaPrivateKey);
                    result = output.ToArray();
                }
            }
            finally
            {
                output.Close();
            }
            return result;
        }

        private static void DecryptData(Stream output, Stream input, RSACryptoServiceProvider rsaPrivateKey)
        {
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.KeySize = 256;
                aesManaged.BlockSize = 128;
                aesManaged.Mode = CipherMode.CBC;

                byte[] LenK = new byte[4];
                byte[] LenIV = new byte[4];

                input.Seek(0, SeekOrigin.Begin);
                input.Read(LenK, 0, 3);
                input.Seek(4, SeekOrigin.Begin);
                input.Read(LenIV, 0, 3);

                int lenK = BitConverter.ToInt32(LenK, 0);
                int lenIV = BitConverter.ToInt32(LenIV, 0);

                int startC = lenK + lenIV + 8;
                int lenC = (int)input.Length - startC;

                byte[] KeyEncrypted = new byte[lenK];
                byte[] IV = new byte[lenIV];

                input.Seek(8, SeekOrigin.Begin);
                input.Read(KeyEncrypted, 0, lenK);
                input.Seek(8 + lenK, SeekOrigin.Begin);
                input.Read(IV, 0, lenIV);

                byte[] KeyDecrypted = rsaPrivateKey.Decrypt(KeyEncrypted, false);

                using (ICryptoTransform transform = aesManaged.CreateDecryptor(KeyDecrypted, IV))
                {
                    int count = 0;
                    int offset = 0;

                    int blockSizeBytes = aesManaged.BlockSize / 8;
                    byte[] data = new byte[blockSizeBytes];

                    input.Seek(startC, SeekOrigin.Begin);
                    using (CryptoStream streamDecrypted = new CryptoStream(output, transform, CryptoStreamMode.Write))
                    {
                        do
                        {
                            count = input.Read(data, 0, blockSizeBytes);
                            offset += count;
                            streamDecrypted.Write(data, 0, count);

                        }
                        while (count > 0);

                        streamDecrypted.FlushFinalBlock();
                        streamDecrypted.Close();
                    }
                    input.Close();
                }
            }
        }
    }
}