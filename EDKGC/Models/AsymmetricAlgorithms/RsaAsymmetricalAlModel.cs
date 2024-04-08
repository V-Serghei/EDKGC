using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using EDKGC.Encryption.RSA;
using Newtonsoft.Json;

namespace EDKGC.Models.AsymmetricAlgorithms
{
    public class RsaAsymmetricalAlModel
    {
        readonly Encoding encoding = Encoding.Default;
        private GenerateKeysRSA _generateKeysRsa = new GenerateKeysRSA(); 

        private RSAParameters _rsaParameters = new RSAParameters();
        private RSA _rsaServiceProvider = RSA.Create();


        public (RSAParameters publicKey, RSAParameters privateKey) Keys { get; set; }

        public byte[] KeyPublic { get; set; }
        
        public byte[] KeyPrivate { get; set; }

        private byte[] ResKeyPublic { get; set; }

        private byte[] ResKeyPrivate { get; set; }

        public string EnterText { get; set; }

        public byte[] EncryptedText { get; set; }

        public byte[] IV { get; set; }

        private void GenerateKeysRsa()
        {
            Keys = _generateKeysRsa.GenerateKeys();
        }

        public byte[] GenPublicKey()
        {
            GenerateKeysRsa();

            var publicKeyBytes = SerializeRsaParameters(Keys.publicKey);
            var privateKeyBytes = SerializeRsaParameters(Keys.privateKey);

            KeyPublic = publicKeyBytes;
            KeyPrivate = privateKeyBytes;

            return KeyPublic;
        }

        public byte[] GenPrivateKey()
        {
            return KeyPrivate;
        }

        public byte[] GetEncryptTextRsa(string enterText)
        {
            byte[] plainBytes = Encoding.Default.GetBytes(enterText);
            EncryptedText = EncryptRSA.Encrypt(plainBytes, Keys.publicKey);
            EnterText = enterText;
            return EncryptedText;
        }

        public string GetDecryptTextRsa(byte[] encryptedBytes)
        {
            var decryptedBytes = DecryptRSA.Decrypt(encryptedBytes, Keys.privateKey);
            return decryptedBytes;
        }
        private byte[] SerializeRsaParameters(RSAParameters parameters)
        {
            string parametersJson = JsonConvert.SerializeObject(parameters);
            return Encoding.Default.GetBytes(parametersJson);
        }

    }
}
