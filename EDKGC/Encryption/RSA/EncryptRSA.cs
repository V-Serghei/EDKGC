using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System.Text;

namespace EDKGC.Encryption.RSA
{
    public class EncryptRSA
    {
        static readonly Encoding _encoding = Encoding.Default;

        public static byte[] EncryptText(string plaintext, AsymmetricCipherKeyPair _keyPair)
        {
            var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
            cipher.Init(true, _keyPair.Public);
            var inputBytes = _encoding.GetBytes(plaintext);
            return cipher.DoFinal(inputBytes);
        }

    }
}
