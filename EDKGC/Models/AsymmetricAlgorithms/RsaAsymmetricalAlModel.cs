using System.Text;
using EDKGC.Enams;
using EDKGC.Encryption.RSA;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace EDKGC.Models.AsymmetricAlgorithms
{
    public class RsaAsymmetricalAlModel
    {
        readonly Encoding encoding = Encoding.Default;
        private readonly GenerateKeysRSA _generateKeysRsa = new GenerateKeysRSA();
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

        public AsymmetricCipherKeyPair GetKeyP()
        {
            return _keyPair;
        }

        public void GenerateKeysRsa()
        {
            _keyPair = _generateKeysRsa.GenerateKeyPair();

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
        //TODO: Consider the maximum length of the text. Define deadening
        public byte[] EncryptTextRsa(string plaintext)
        {
            EnterText = plaintext;
            EncryptedText = EncryptRsa.EncryptText(plaintext, _keyPair, KeyEnDe);
            return EncryptedText;

        }
        public byte[] EncryptTextRsa(byte[] plaintext)
        {
            EnterText = encoding.GetString(plaintext);
            EncryptedText = EncryptRsa.EncryptTextBytes(plaintext, _keyPair, KeyEnDe);
            return EncryptedText;

        }


        public string DecryptTextRsa(byte[] encryptedBytes)
        {
           
            return DecryptRsa.DecryptRsaT(encryptedBytes, _keyPair, KeyEnDe);
        }
        public byte[] DecryptTextRsaB(byte[] encryptedBytes)
        {

            return DecryptRsa.DecryptRsaToByte(encryptedBytes, _keyPair, KeyEnDe);
        }



    }
}
