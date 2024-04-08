using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    namespace EDKGC.Encryption.RSA
    {
        public class DecryptRSA
        {
            public static string Decrypt(byte[] encryptedBytes, RSAParameters privateKey)
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(privateKey); 

                    byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, true); 
                    string decryptedText = Encoding.Default.GetString(decryptedBytes);
                    return decryptedText;
                }
            }
        }
    }
