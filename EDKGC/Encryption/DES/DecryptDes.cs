using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EDKGC.Encryption.DES
{
    public static class DecryptDes
    {
        public static string DecryptEbc(byte[] cipherText, byte[] key)
        {
            using var des = System.Security.Cryptography.DES.Create();
            des.Key = key;
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros;

            using var memoryStream = new MemoryStream(cipherText);
            using var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Read);
            using var decryptedMemoryStream = new MemoryStream();
            cryptoStream.CopyTo(decryptedMemoryStream);

            var decryptedText = Encoding.Default.GetString(decryptedMemoryStream.ToArray());
            return decryptedText.TrimEnd('\0');
        }


        public static string DecryptCbc(byte[] cipherText, byte[] key, byte[] iv)
        {
            using var des = System.Security.Cryptography.DES.Create();
            des.Key = key;
            des.IV = iv;
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;

            using var memoryStream = new MemoryStream(cipherText);
            using var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Read);
            var decryptedBytes = new byte[cipherText.Length];
            var decryptedByteCount = cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
            return Encoding.Default.GetString(decryptedBytes, 0, decryptedByteCount).TrimEnd('\0');
        }


    }
}
