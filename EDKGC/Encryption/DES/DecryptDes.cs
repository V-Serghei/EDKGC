using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EDKGC.Encryption.DES
{
    public class DecryptDes
    {
        public static string DecryptEbc(byte[] cipherText, byte[] key)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.Mode = CipherMode.ECB;
                des.Padding = PaddingMode.Zeros;

                using (var memoryStream = new MemoryStream(cipherText))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        // Создаем временный буфер для чтения данных
                        byte[] buffer = new byte[cipherText.Length];
                        int bytesRead;
                        using (var memoryStreamResult = new MemoryStream())
                        {
                            // Читаем все данные из CryptoStream во временный буфер
                            while ((bytesRead = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                memoryStreamResult.Write(buffer, 0, bytesRead);
                            }
                            // Преобразуем результат в строку и возвращаем
                            return Encoding.UTF8.GetString(memoryStreamResult.ToArray()).TrimEnd('\0');
                        }
                    }
                }
            }
        }


        public static string DecryptCbc(byte[] cipherText, byte[] key, byte[] iv)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv;
                des.Mode = CipherMode.CBC; 
                des.Padding = PaddingMode.PKCS7; 

                using (var memoryStream = new System.IO.MemoryStream(cipherText))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        byte[] decryptedBytes = new byte[cipherText.Length];
                        int decryptedByteCount = cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                        return Encoding.Default.GetString(decryptedBytes, 0, decryptedByteCount).TrimEnd('\0'); 
                    }
                }
            }
        }


    }
}
