using System.Text;
using EDKGC.Enams;
using EDKGC.Encryption.DES;
using EDKGC.Encryption.RSA;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace EDKGC.Models.AsymmetricAlgorithms
{
    public class RsaAsymmetricalAlModel
    {
        readonly Encoding encoding = Encoding.Default;
        private GenerateKeysRSA _generateKeysRsa = new GenerateKeysRSA();
        private AsymmetricCipherKeyPair _keyPair;

        public RsaAsymmetricalAlModel()
        {
            GenerateKeysRsa();
            KeyEnDe = EKeyEff.Public;
        }


        public EKeyEff KeyEnDe { get; set; }

        public byte[] KeyPublic { get; set; }
        
        public byte[] KeyPrivate { get; set; }

        private byte[] ResKeyPublic { get; set; }

        private byte[] ResKeyPrivate { get; set; }

        public string EnterText { get; set; }

        public byte[] EncryptedText { get; set; }

        public byte[] IV { get; set; }

        public void GenerateKeysRsa()
        {
            var keyGenerationParameters = new KeyGenerationParameters(new SecureRandom(), 2048);
            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);
            _keyPair = keyPairGenerator.GenerateKeyPair();

            KeyPublic = ((RsaKeyParameters)_keyPair.Public).Modulus.ToByteArrayUnsigned();
            KeyPrivate = ((RsaPrivateCrtKeyParameters)_keyPair.Private).Exponent.ToByteArrayUnsigned();
        }

        public byte[] GenPublicKey()
        {
            return KeyPublic = ((RsaKeyParameters)_keyPair.Public).Modulus.ToByteArrayUnsigned();
        }

        public byte[] GenPrivateKey()
        {
            return KeyPrivate = ((RsaPrivateCrtKeyParameters)_keyPair.Private).Exponent.ToByteArrayUnsigned();
        }

        public byte[] EncryptTextRsa(string plaintext)
        {
            EnterText = plaintext;
            EncryptedText = EncryptRSA.EncryptText(plaintext, _keyPair,KeyEnDe);
            return EncryptedText;

        }


        public string DecryptTextRsa(byte[] encryptedBytes)
        {
           
            return DecryptRSA.DecryptRsaT(encryptedBytes,_keyPair, KeyEnDe);
        }



    }
}
