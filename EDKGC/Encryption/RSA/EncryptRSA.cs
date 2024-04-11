using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System.Text;
using EDKGC.Enams;

namespace EDKGC.Encryption.RSA
{
    public class EncryptRSA
    {
        static readonly Encoding _encoding = Encoding.Default;

        public static byte[] EncryptText(string plaintext, AsymmetricCipherKeyPair _keyPair, EKeyEff state)
        {
            var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
           if(state == EKeyEff.Public) cipher.Init(true, _keyPair.Public);
           if (state == EKeyEff.Private) cipher.Init(true, _keyPair.Private);
            var inputBytes = _encoding.GetBytes(plaintext);
            return cipher.DoFinal(inputBytes);
        }
        public static byte[] EncryptTextBytes(byte[] plaintext, AsymmetricCipherKeyPair _keyPair, EKeyEff state)
        {
            var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
            if (state == EKeyEff.Public) cipher.Init(true, _keyPair.Public);
            if (state == EKeyEff.Private) cipher.Init(true, _keyPair.Private);
            var inputBytes = (plaintext);
            return cipher.DoFinal(inputBytes);
        }

    }
}
