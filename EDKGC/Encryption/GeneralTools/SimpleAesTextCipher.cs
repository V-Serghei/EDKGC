using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EDKGC.Encryption.GeneralTools
{
    public static class SimpleAesTextCipher
    {
        public static byte[] Encrypt(string text, byte[] keyMaterial)
        {
            using var aes = Aes.Create();
            aes.Key = SHA256.HashData(keyMaterial);
            aes.GenerateIV();

            using var stream = new MemoryStream();
            stream.Write(aes.IV, 0, aes.IV.Length);
            using (var cryptoStream = new CryptoStream(stream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                var textBytes = Encoding.UTF8.GetBytes(text);
                cryptoStream.Write(textBytes, 0, textBytes.Length);
                cryptoStream.FlushFinalBlock();
            }

            return stream.ToArray();
        }

        public static string Decrypt(byte[] encryptedText, byte[] keyMaterial)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = SHA256.HashData(keyMaterial);
                byte[] iv = new byte[aes.BlockSize / 8];
                Array.Copy(encryptedText, iv, iv.Length);
                aes.IV = iv;

                using (var input = new MemoryStream(encryptedText, iv.Length, encryptedText.Length - iv.Length))
                using (var cryptoStream = new CryptoStream(input, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var output = new MemoryStream())
                {
                    cryptoStream.CopyTo(output);
                    return Encoding.UTF8.GetString(output.ToArray());
                }
            }
        }
    }
}
