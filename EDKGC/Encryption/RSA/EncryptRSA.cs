using System.Text;
using EDKGC.Enams;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace EDKGC.Encryption.RSA
{
    public class EncryptRSA
    {
        static readonly Encoding _encoding = Encoding.UTF8;

        public static byte[] EncryptText(string plaintext, AsymmetricCipherKeyPair _keyPair, EKeyEff state)
        {
            try
            {
                var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
                if (state == EKeyEff.Public) cipher.Init(true, _keyPair.Public);
                if (state == EKeyEff.Private) cipher.Init(true, _keyPair.Private);
                return cipher.DoFinal(_encoding.GetBytes(plaintext));
            }
            catch (DataLengthException)
            {
                return null;
            }
        }

        public static byte[] EncryptTextBytes(byte[] plaintext, AsymmetricCipherKeyPair _keyPair, EKeyEff state)
        {
            try
            {
                var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
                if (state == EKeyEff.Public) cipher.Init(true, _keyPair.Public);
                if (state == EKeyEff.Private) cipher.Init(true, _keyPair.Private);
                return cipher.DoFinal(plaintext);
            }
            catch (DataLengthException)
            {
                return null;
            }
        }
    }
}
