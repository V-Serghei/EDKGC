using System.Linq;
using System.Text;
using EDKGC.Encryption.GeneralTools;
using Org.BouncyCastle.Crypto;

namespace EDKGC.Models.SymmetricEncryption
{
    public abstract class BouncyBlockSymmetricEncryption
    {
        readonly Encoding _encoding = Encoding.UTF8;

        public byte[] Key { get; set; }
        public byte[] ResKey { get; set; }
        public string EnterText { get; set; }
        public byte[] EncryptedText { get; set; }

        protected abstract int KeyLength { get; }
        protected abstract IBlockCipher CreateEngine();

        public byte[] GenKey()
        {
            Key = GenerateSymmetricKey.GenKey(KeyLength);
            return Key;
        }

        public byte[] GetEncryptTextEdc(string notEncryptText)
        {
            if (notEncryptText == null) return null;
            if (Key == null) GenKey();

            EnterText = notEncryptText;
            ResKey = Key?.ToArray();
            EncryptedText = BlockCipherTools.EncryptEbc(_encoding.GetBytes(notEncryptText), Key, CreateEngine());
            return EncryptedText;
        }

        public string GetDecryptTextEbc(string encryptText)
        {
            if (Key == null || ResKey == null || !Key.SequenceEqual(ResKey))
                return null;

            var byteText = BlockCipherTools.DecryptEbc(EncryptedText, Key, CreateEngine());
            return _encoding.GetString(byteText);
        }
    }
}
