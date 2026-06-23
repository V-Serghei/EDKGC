namespace EDKGC.Encryption.RSA
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Generators;
    using Org.BouncyCastle.Security;

    public class GenerateKeysRSA
    {
        public AsymmetricCipherKeyPair GenerateKeyPair(int keySize = 2048)
        {
            var keyGenerationParameters = new KeyGenerationParameters(new SecureRandom(), keySize);
            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);
            return keyPairGenerator.GenerateKeyPair();
        }
    }
}
