using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace EDKGC.Encryption.AES
{
    public static class GenerateKeyAes
    {
        public static byte[] GenerateAesKey(int keySize)
        {
            var keyGenerator = GeneratorUtilities.GetKeyGenerator("AES");

            var keyGenParam = new KeyGenerationParameters(new SecureRandom(), keySize);

            keyGenerator.Init(keyGenParam);
            return keyGenerator.GenerateKey();
        }

    }
}
