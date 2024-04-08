using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDKGC.Encryption.AES;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

namespace EDKGC.Models
{
    public  class AesSymmetricEncryption
    {
        readonly Encoding encoding = Encoding.UTF8;

        public byte[] Key { get; set; }

        public string EnterText { get; set; }

        public byte[] EncryptedText { get; set; }

        public byte[] IV { get; set; }

        // AesSymmetricEncryption _aseSe = new AesSymmetricEncryption();
        public AesSymmetricEncryption()
        {

        }


        public byte[] GenKeyAesAlg()
        {

            GenerateKeyAes _genKey = new GenerateKeyAes();

            Key = _genKey.GenerateAesKey(128);
            return Key;
        }


       public byte[] Encrypting(string entText)
        {
            EncryptAes _encrypt = new EncryptAes();
            EnterText = entText;


            var encryptedText = encoding.GetBytes(EnterText);
            EncryptedText = _encrypt.Encrypt(encryptedText, Key);

            return EncryptedText;
        }

        public byte[] EncryptingCbc(string entText)
        {
            EncryptAes _encrypt = new EncryptAes();
            EnterText = entText;
            var encryptedText = Convert.FromBase64String(EnterText);
            EncryptedText = _encrypt.EncryptIV(encryptedText, Key, IV);

            return EncryptedText;
        }

        public byte[] Decrypt(string entText)
        {
            DecryptAes _decrypt = new DecryptAes();



            return _decrypt.Decrypt(EncryptedText, Key);
          

        }


    }
}
