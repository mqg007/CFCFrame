using System;

namespace Common.Cryptography
{
    public class BKDRManaged : System.Security.Cryptography.HashAlgorithm
    {
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            int seed = 131;
            int hash = this.HashValue == null || this.HashValue.Length < 4 ? 0 : BitConverter.ToInt32(this.HashValue, 0);
            for (int i = ibStart; i < cbSize; i++)
            {
                hash = (hash * seed) + array[i];
            }
            this.HashValue = BitConverter.GetBytes(hash & 0x7FFFFFFF);
        }

        protected override byte[] HashFinal()
        {
            return this.HashValue;
        }

        public override void Initialize()
        {
        }
    }
}