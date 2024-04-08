using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EDKGC.Encryption.RSA
{
    public class EncryptRSA
    {
        public static byte[] Encrypt(byte[] enterText, RSAParameters publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(publicKey); 

                byte[] plainBytes = enterText;
                byte[] encryptedBytes = rsa.Encrypt(plainBytes, true); 
                return encryptedBytes;
            }
        }
    }
}
