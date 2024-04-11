using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDKGC.Encryption.GeneralTools;
using System.Security.Cryptography;
using EDKGC.Enams;
using EDKGC.Encryption.RSA;

namespace EDKGC.Models.ElectronicSignature
{
    public class ElectronicSignatureRSA
    {
        private AsymmetricCipherKeyPair _keyPair;
        readonly Encoding _encoding = Encoding.Default;


        public string EnterText { get; set; }

        public byte[] EncryptedText { get; set; }

        public byte[] HashBytes { get; set; }

        public string HashPriv { get; set; }

        private string EncryptHash { get; set; }

        private string DecryptHash { get; set; }

        public byte[] GetHashBytes()
        {
            return HashBytes;
        }

        public void SetKeyPair(AsymmetricCipherKeyPair keyP)
        {
            _keyPair = keyP;
        }

        public string GenHashPrivKey(string enterTextH)
        {
            if (enterTextH == null) throw new ArgumentNullException(nameof(enterTextH));
            HashBytes = GenHash.GenHashText(enterTextH);
            
            HashPriv = GetHexModString.GetHexModToString(HashBytes);
            return HashPriv;
        }

        public string EncryptHashText(string enterTextH)
        {
            var text = EncryptRSA.EncryptTextBytes(HashBytes, _keyPair, EKeyEff.Private);
            EncryptedText = text;

            return GetHexModString.GetHexModToString(text);
        }

        public string DecryptTextHash()
        {
            
            var nEncrypt = DecryptRSA.DecryptRsaToByte(EncryptedText, _keyPair,EKeyEff.Public);

            DecryptHash = _encoding.GetString(nEncrypt);
            return GetHexModString.GetHexModToString(nEncrypt);

        }

        public string VerifySignature(string signature, string dHash)
        {
            var resp = (signature == dHash) ? "Pass" : "Hazard!!!";
            return resp;

        }



    }
}
