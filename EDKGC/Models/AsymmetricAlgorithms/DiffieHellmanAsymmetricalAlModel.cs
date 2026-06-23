using EDKGC.Encryption.GeneralTools;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace EDKGC.Models.AsymmetricAlgorithms
{
    public class DiffieHellmanAsymmetricalAlModel : ISharedSecretAsymmetricalAlModel
    {
        private AsymmetricCipherKeyPair _localKeyPair;
        private AsymmetricCipherKeyPair _peerKeyPair;
        private byte[] _sharedSecret;

        public byte[] EncryptedText { get; set; }

        public void GenerateKeys()
        {
            var random = new SecureRandom();
            var parametersGenerator = new DHParametersGenerator();
            parametersGenerator.Init(512, 20, random);
            var parameters = parametersGenerator.GenerateParameters();

            var keyPairGenerator = new DHKeyPairGenerator();
            keyPairGenerator.Init(new DHKeyGenerationParameters(random, parameters));
            _localKeyPair = keyPairGenerator.GenerateKeyPair();
            _peerKeyPair = keyPairGenerator.GenerateKeyPair();
            _sharedSecret = CalculateSharedSecret(_localKeyPair.Private, _peerKeyPair.Public);
        }

        public byte[] GetPublicKey()
        {
            return ((DHPublicKeyParameters)_localKeyPair.Public).Y.ToByteArrayUnsigned();
        }

        public byte[] GetPrivateKey()
        {
            return ((DHPrivateKeyParameters)_localKeyPair.Private).X.ToByteArrayUnsigned();
        }

        public byte[] GetSharedSecret()
        {
            return _sharedSecret;
        }

        public byte[] EncryptText(string plaintext)
        {
            if (_sharedSecret == null) GenerateKeys();
            EncryptedText = SimpleAesTextCipher.Encrypt(plaintext, _sharedSecret);
            return EncryptedText;
        }

        public string DecryptText(byte[] encryptedBytes)
        {
            return SimpleAesTextCipher.Decrypt(encryptedBytes, _sharedSecret);
        }

        private static byte[] CalculateSharedSecret(ICipherParameters privateKey, ICipherParameters publicKey)
        {
            var agreement = new DHBasicAgreement();
            agreement.Init(privateKey);
            return agreement.CalculateAgreement(publicKey).ToByteArrayUnsigned();
        }
    }
}
