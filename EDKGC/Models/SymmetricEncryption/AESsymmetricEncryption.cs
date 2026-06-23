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
            Key = new GenerateKeyAes().GenerateAesKey(128);
            return Key;
        }

        public byte[] Encrypting(string entText)
        {
            if (entText == null) return null;
            if (Key == null) GenKeyAesAlg();
            EnterText = entText;
            ResKey = Key?.ToArray();
            EncryptedText = new EncryptAes().Encrypt(_encoding.GetBytes(EnterText), Key);
            return EncryptedText;
        }

        public byte[] EncryptingCbc(string entText)
        {
            EnterText = entText;
            EncryptedText = new EncryptAes().EncryptIV(Convert.FromBase64String(EnterText), Key, IV);
            return EncryptedText;
        }

        public byte[] Decrypt(string entText)
        {
            if (Key == null || ResKey == null || !Key.SequenceEqual(ResKey))
                return null;
            return new DecryptAes().Decrypt(EncryptedText, Key);
        }
    }
}
