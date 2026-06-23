using System.IO;
using System.Security.Cryptography;

namespace EDKGC.Encryption.AES
{
    public class EncryptAes
    {
        public byte[] Encrypt(byte[] plainText, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.ECB; 
                aes.Key = key;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(plainText, 0, plainText.Length);
                }
            }
        }
        public byte[] EncryptIV(byte[] plainText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Key = key;
                aes.GenerateIV();
                iv = aes.IV;
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(aes.IV, 0, aes.IV.Length);

                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
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
