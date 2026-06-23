using System.Text;
using EDKGC.Enams;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace EDKGC.Encryption.RSA
{
    public static class DecryptRsa
    {
        private static readonly Encoding Encoding = System.Text.Encoding.UTF8;

        public static string DecryptRsaT(byte[] encryptedBytes, AsymmetricCipherKeyPair keyPair, EKeyEff stat)
        {
            try
            {
                var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
                if (stat == EKeyEff.Public) cipher.Init(false, keyPair.Private);
                if (stat == EKeyEff.Private) cipher.Init(false, keyPair.Public);
                return Encoding.GetString(cipher.DoFinal(encryptedBytes));
            }
            catch (InvalidCipherTextException)
            {
                return null;
            }
        }

        public static byte[] DecryptRsaToByte(byte[] encryptedBytes, AsymmetricCipherKeyPair keyPair, EKeyEff stat)
        {
            try
            {
                var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
                if (stat == EKeyEff.Public) cipher.Init(false, keyPair.Private);
                if (stat == EKeyEff.Private) cipher.Init(false, keyPair.Public);
                return cipher.DoFinal(encryptedBytes);
            }
            catch (InvalidCipherTextException)
            {
                return null;
            }
        }
    }
}
