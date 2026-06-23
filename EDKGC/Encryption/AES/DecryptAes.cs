using System.IO;
using System.Security.Cryptography;

namespace EDKGC.Encryption.AES
{
    public static class DecryptAes
    {
        public static byte[] Decrypt(byte[] cipherText, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.ECB;
                aes.Key = key;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    return decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                }
            }
        }

        public static byte[] DecryptCbc(byte[] cipherText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Key = key;
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream(cipherText))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (MemoryStream decryptedMs = new MemoryStream())
                            {
                                cs.CopyTo(decryptedMs);
                                return decryptedMs.ToArray();
                            }
                        }
                    }
                }
            }
        }
    }
    

}
