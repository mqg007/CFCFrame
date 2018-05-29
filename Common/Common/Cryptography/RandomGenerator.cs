using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Common.Cryptography
{
    public sealed partial class RandomGenerator
    {
        public static byte[] GetBytes(int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException();

            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] buffer = new byte[count];
                rng.GetBytes(buffer);
                return buffer;
            }
        }

        public static int GetInt32(int maxValue)
        {
            Random rnd = new Random(GetInt32());
            return rnd.Next(maxValue);
        }

        public static int GetInt32(int minValue, int maxValue)
        {
            Random rnd = new Random(GetInt32());
            return rnd.Next(minValue, maxValue);
        }

        public static int GetInt32()
        {
            return BitConverter.ToInt32(GetBytes(4), 0);
        }

        public static int[] GetInt32Array(int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException();

            byte[] buffer = GetBytes(count * 4);
            int[] result = new int[count];
            for (int i = 0; i < count; i++)
            {
                result[i] = BitConverter.ToInt32(buffer, i);
            }
            return result;
        }

        public static long GetInt64()
        {
            return BitConverter.ToInt64(GetBytes(8), 0);
        }

        public static double GetDouble()
        {
            return BitConverter.ToDouble(GetBytes(8), 0);
        }

        public static string GetString()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public static string GetString(int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException();

            return Convert.ToBase64String(GetBytes(count)).Substring(0, count);
        }
    }
}