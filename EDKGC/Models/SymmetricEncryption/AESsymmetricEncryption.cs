using System;
using System.Linq;
using System.Text;
using EDKGC.Encryption.AES;

namespace EDKGC.Models
{
    public class AesSymmetricEncryption
    {
        readonly Encoding _encoding = Encoding.UTF8;

        public byte[] Key { get; set; }
        public byte[] ResKey { get; set; }
        public string EnterText { get; set; }
        public byte[] EncryptedText { get; set; }
        public byte[] IV { get; set; }

        public byte[] GenKeyAesAlg()
        {
            Key = GenerateKeyAes.GenerateAesKey(128);
            return Key;
        }

        public byte[] Encrypting(string entText)
        {
            if (entText == null) return null;
            if (Key == null) GenKeyAesAlg();
            EnterText = entText;
            ResKey = Key?.ToArray();
            EncryptedText = EncryptAes.Encrypt(_encoding.GetBytes(EnterText), Key);
            return EncryptedText;
        }

        public byte[] EncryptingCbc(string entText)
        {
            EnterText = entText;
            EncryptedText = EncryptAes.EncryptIv(Convert.FromBase64String(EnterText), Key);
            return EncryptedText;
        }

        public byte[] Decrypt(string entText)
        {
            if (Key == null || ResKey == null || !Key.SequenceEqual(ResKey))
                return null;
            return DecryptAes.Decrypt(EncryptedText, Key);
        }
    }
}
