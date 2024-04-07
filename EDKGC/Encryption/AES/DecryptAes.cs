using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EDKGC.Encryption.AES
{
    public class DecryptAes
    {
        public static byte[] Decrypt(byte[] ciphertext, byte[] key)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            ICryptoTransform decryptor = aes.CreateDecryptor(key, null);
            byte[] plaintext = new byte[ciphertext.Length];

            int processedBytes = decryptor.TransformBlock(ciphertext, 0, ciphertext.Length, plaintext, 0);
            decryptor.TransformFinalBlock(ciphertext, processedBytes, ciphertext.Length - processedBytes);

            return plaintext;
        }

    }
}
