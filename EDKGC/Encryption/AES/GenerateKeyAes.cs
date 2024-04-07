using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDKGC.Encryption.AES
{
    public class GenerateKeyAes
    {
        public byte[] GenerateAesKey(int keySize)
        {
            CipherKeyGenerator keyGenerator = GeneratorUtilities.GetKeyGenerator("AES");

            KeyGenerationParameters keyGenParam = new KeyGenerationParameters(new SecureRandom(), keySize);

            keyGenerator.Init(keyGenParam);
            return keyGenerator.GenerateKey();
        }
    }
}
