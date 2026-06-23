using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

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
