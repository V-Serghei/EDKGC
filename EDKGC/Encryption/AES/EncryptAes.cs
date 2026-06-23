using System.IO;
using System.Security.Cryptography;

namespace EDKGC.Encryption.AES
{
    public static class EncryptAes
    {
        public static byte[] Encrypt(byte[] plainText, byte[] key)
        {
            using var aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
            aes.Key = key;

            using var encryptor = aes.CreateEncryptor();
            return encryptor.TransformFinalBlock(plainText, 0, plainText.Length);
        }

        public static byte[] EncryptIv(byte[] plainText, byte[] key)
        {
            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Key = key;
                aes.GenerateIV();
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                        ms.Write(aes.IV, 0, aes.IV.Length);

                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(plainText, 0, plainText.Length);
                        }

                        return ms.ToArray();
                    }
                }
            }
        }
    }
}
