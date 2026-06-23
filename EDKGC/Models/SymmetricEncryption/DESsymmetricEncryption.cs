using System.Linq;
using System.Text;
using EDKGC.Encryption.DES;

namespace EDKGC.Models.SymmetricEncryption
{
    public class DESsymmetricEncryption
    {
        readonly Encoding _encoding = Encoding.UTF8;

        public byte[] Key { get; set; }
        public byte[] ResKey { get; set; }
        public string EnterText { get; set; }
        public byte[] EncryptedText { get; set; }
        public byte[] IV { get; set; }

        public DESsymmetricEncryption()
        {
            GenKeyDes();
        }

        public byte[] GenKeyDes()
        {
            Key = GenerateKeyDes.GenKeyDes();
            return Key;
        }

        public void GenIvDes()
        {
            IV = GenerateKeyDes.GenIv();
        }

        public byte[] GetEncryptTextEdc(string notEncryptText)
        {
            EnterText = notEncryptText;
            ResKey = Key?.ToArray();
            EncryptedText = EncryptDes.EncryptDesEbc(_encoding.GetBytes(notEncryptText), Key);
            return EncryptedText;
        }

        public byte[] GetEncryptTextCbc(string notEncryptText)
        {
            EnterText = notEncryptText;
            ResKey = Key?.ToArray();
            EncryptedText = EncryptDes.EncryptDesCbc(_encoding.GetBytes(notEncryptText), Key, IV);
            return EncryptedText;
        }

        public string GetDecryptTextEbc(string encryptText)
        {
            if (Key == null || ResKey == null || !Key.SequenceEqual(ResKey))
                return null;
            return DecryptDes.DecryptEbc(EncryptedText, Key);
        }

        public string GetDecryptTextCbc(string encryptText)
        {
            if (Key == null || ResKey == null || !Key.SequenceEqual(ResKey))
                return null;
            return DecryptDes.DecryptCbc(_encoding.GetBytes(encryptText), Key, IV);
        }
    }
}
