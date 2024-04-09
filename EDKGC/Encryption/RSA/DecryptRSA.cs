using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Security;

    namespace EDKGC.Encryption.RSA
    {
        public class DecryptRSA
        {
            static readonly Encoding _encoding = Encoding.Default;

        public static string DecryptRsaT(byte[] encryptedBytes, AsymmetricCipherKeyPair _keyPair)
            {
                var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
                cipher.Init(false, _keyPair.Private);

                var decryptedBytes = cipher.DoFinal(encryptedBytes);
                return _encoding.GetString(decryptedBytes);
            }
    }
    }
