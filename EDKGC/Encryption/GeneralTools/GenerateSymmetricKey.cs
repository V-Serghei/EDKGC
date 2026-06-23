using System.Security.Cryptography;

namespace EDKGC.Encryption.GeneralTools
{
    public static class GenerateSymmetricKey
    {
        public static byte[] GenKey(int length)
        {
            byte[] key = new byte[length];
            RandomNumberGenerator.Fill(key);
            return key;
        }
    }
}
