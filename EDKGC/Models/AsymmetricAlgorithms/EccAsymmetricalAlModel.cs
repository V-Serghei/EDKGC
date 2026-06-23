using EDKGC.Encryption.GeneralTools;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace EDKGC.Models.AsymmetricAlgorithms
{
    public class EccAsymmetricalAlModel : ISharedSecretAsymmetricalAlModel
    {
        private AsymmetricCipherKeyPair _localKeyPair;
        private AsymmetricCipherKeyPair _peerKeyPair;
        private byte[] _sharedSecret;

        public byte[] EncryptedText { get; set; }

        public void GenerateKeys()
        {
            var random = new SecureRandom();
            var curve = SecNamedCurves.GetByName("secp256r1");
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H, curve.GetSeed());

            var keyPairGenerator = new ECKeyPairGenerator();
            keyPairGenerator.Init(new ECKeyGenerationParameters(domain, random));
            _localKeyPair = keyPairGenerator.GenerateKeyPair();
            _peerKeyPair = keyPairGenerator.GenerateKeyPair();
            _sharedSecret = CalculateSharedSecret(_localKeyPair.Private, _peerKeyPair.Public);
        }

        public byte[] GetPublicKey()
        {
            return ((ECPublicKeyParameters)_localKeyPair.Public).Q.GetEncoded(false);
        }

        public byte[] GetPrivateKey()
        {
            return ((ECPrivateKeyParameters)_localKeyPair.Private).D.ToByteArrayUnsigned();
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
            var agreement = new ECDHBasicAgreement();
            agreement.Init(privateKey);
            return agreement.CalculateAgreement(publicKey).ToByteArrayUnsigned();
        }
    }
}
