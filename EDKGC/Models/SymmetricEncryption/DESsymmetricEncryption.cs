using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDKGC.Encryption.DES;

namespace EDKGC.Models.SymmetricEncryption
{
    public class DESsymmetricEncryption
    {
        readonly Encoding _encoding = Encoding.Default;

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
            var byteText = _encoding.GetBytes(notEncryptText);
            ResKey = Key;
            EncryptedText = EncryptDes.EncryptDesEbc(byteText, Key);
            return EncryptedText;
        }
        public byte[] GetEncryptTextCbc(string notEncryptText)
        {
            EnterText = notEncryptText;
            var byteText = _encoding.GetBytes(notEncryptText);
            ResKey = Key;
            EncryptedText = EncryptDes.EncryptDesCbc(byteText, Key,IV);
            return EncryptedText;
        }

        public string GetDecryptTextEbc(string encryptText)
        {
            if (ResKey == Key)
            {
                var byteText = _encoding.GetBytes(encryptText);
                return DecryptDes.DecryptEbc(byteText, Key);

            }
            else return null;


        }

        public string GetDecryptTextCbc(string encryptText)
        {
            if (ResKey == Key)
            {
                var byteText = _encoding.GetBytes(encryptText);
                return DecryptDes.DecryptCbc(byteText, Key,IV);

            }
            else return null;


        }
    }
}
