using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EDKGC.Enams;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace EDKGC.Encryption.RSA
{
    public class DecryptRSA
    {
        static readonly Encoding _encoding = Encoding.Default;

        public static string DecryptRsaT(byte[] encryptedBytes, AsymmetricCipherKeyPair _keyPair, EKeyEff stat)
        {
            var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
            if(stat == EKeyEff.Public)cipher.Init(false, _keyPair.Private);
            if (stat == EKeyEff.Private) cipher.Init(false, _keyPair.Public);

            var decryptedBytes = cipher.DoFinal(encryptedBytes);
            return _encoding.GetString(decryptedBytes);
        }
        public static byte[] DecryptRsaToByte(byte[] encryptedBytes, AsymmetricCipherKeyPair _keyPair, EKeyEff stat)
        {
            var cipher = CipherUtilities.GetCipher("RSA/NONE/PKCS1Padding");
            if (stat == EKeyEff.Public) cipher.Init(false, _keyPair.Private);
            if (stat == EKeyEff.Private) cipher.Init(false, _keyPair.Public);

            var decryptedBytes = cipher.DoFinal(encryptedBytes);
            return decryptedBytes;
        }
    }
}
