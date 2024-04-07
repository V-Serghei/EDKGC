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
        public byte[] Encrypt(byte[] entText, byte[] key)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            ICryptoTransform encryptor = aes.CreateEncryptor(key,null);
            byte[] ciphertext = new byte[entText.Length];

            int processedBytes = encryptor.TransformBlock(entText, 0, entText.Length, ciphertext, 0);
            encryptor.TransformFinalBlock(ciphertext, processedBytes, ciphertext.Length - processedBytes);

            return ciphertext;
        }
    }
}
