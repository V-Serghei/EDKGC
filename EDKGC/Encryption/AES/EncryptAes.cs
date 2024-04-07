using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
    }
}
