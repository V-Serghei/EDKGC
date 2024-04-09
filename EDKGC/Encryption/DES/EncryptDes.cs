using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EDKGC.Encryption.DES
{
    public class EncryptDes
    {
        public static byte[] EncryptDesEbc(byte[] enterText, byte[] key)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.Mode = CipherMode.ECB; 
                des.Padding = PaddingMode.Zeros;

                var inputBytes = enterText;
                using (var memoryStream = new System.IO.MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
        }
        public static byte[] EncryptDesCbc(byte[] plainText, byte[] key, byte[] iv)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv;
                des.Mode = CipherMode.CBC; 
                des.Padding = PaddingMode.PKCS7; 
               
                var inputBytes = plainText;
                using (var stream = new System.IO.MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(stream, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(inputBytes, 0, inputBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return stream.ToArray();
                    }
                }
            }
        }

    }
}
