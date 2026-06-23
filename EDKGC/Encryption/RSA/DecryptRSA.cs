using System.Text;
using EDKGC.Enams;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace EDKGC.Encryption.RSA
{
    public class DecryptRSA
    {
        static readonly Encoding _encoding = Encoding.UTF8;

        public static string DecryptRsaT(byte[] encryptedBytes, AsymmetricCipherKeyPair _keyPair, EKeyEff stat)
        {
            var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
            if (stat == EKeyEff.Public) cipher.Init(false, _keyPair.Private);
            if (stat == EKeyEff.Private) cipher.Init(false, _keyPair.Public);
            return _encoding.GetString(cipher.DoFinal(encryptedBytes));
        }

        public static byte[] DecryptRsaToByte(byte[] encryptedBytes, AsymmetricCipherKeyPair _keyPair, EKeyEff stat)
        {
            var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
            if (stat == EKeyEff.Public) cipher.Init(false, _keyPair.Private);
            if (stat == EKeyEff.Private) cipher.Init(false, _keyPair.Public);
            return cipher.DoFinal(encryptedBytes);
        }
    }
}
