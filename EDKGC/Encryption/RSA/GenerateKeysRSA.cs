using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EDKGC.Encryption.RSA
{
    public class GenerateKeysRSA
    {
        private RSACryptoServiceProvider _rsa;
        public  (RSAParameters publicKey, RSAParameters privateKey) GenerateKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                RSAParameters publicKey = rsa.ExportParameters(false); 
                RSAParameters privateKey = rsa.ExportParameters(true); 
                return (publicKey, privateKey); 
            }
        }
        public RSAParameters GeneratePublicKey()
        {
            _rsa = new RSACryptoServiceProvider();
            RSAParameters publicKey = _rsa.ExportParameters(false); 
            return publicKey;
        }
        public RSAParameters GeneratePrivateKey()
        {
            if (_rsa == null)
            {
                throw new InvalidOperationException("Необходимо сначала сгенерировать публичный ключ");
            }

            RSAParameters privateKey = _rsa.ExportParameters(true); 
            return privateKey;
        }


    }
}
