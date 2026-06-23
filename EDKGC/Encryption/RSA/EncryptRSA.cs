using System.Text;
using EDKGC.Enams;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace EDKGC.Encryption.RSA
{
    public static class EncryptRsa
    {
        private static readonly Encoding Encoding = System.Text.Encoding.UTF8;

        public static byte[] EncryptText(string plaintext, AsymmetricCipherKeyPair keyPair, EKeyEff state)
        {
            try
            {
                var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
                switch (state)
                {
                    case EKeyEff.Public:
                        cipher.Init(true, keyPair.Public);
                        break;
                    case EKeyEff.Private:
                        cipher.Init(true, keyPair.Private);
                        break;
                }

                return cipher.DoFinal(Encoding.GetBytes(plaintext));
            }
            catch (DataLengthException)
            {
                return null;
            }
        }

        public static byte[] EncryptTextBytes(byte[] plaintext, AsymmetricCipherKeyPair keyPair, EKeyEff state)
        {
            try
            {
                var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
                if (state == EKeyEff.Public) cipher.Init(true, keyPair.Public);
                if (state == EKeyEff.Private) cipher.Init(true, keyPair.Private);
                return cipher.DoFinal(plaintext);
            }
            catch (DataLengthException)
            {
                return null;
            }
        }
    }
}
