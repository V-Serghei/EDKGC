using System;
using System.Text;
using EDKGC.Enams;
using EDKGC.Encryption.GeneralTools;
using EDKGC.Encryption.RSA;
using Org.BouncyCastle.Crypto;

namespace EDKGC.Models.ElectronicSignature
{
    public class ElectronicSignatureRSA
    {
        private AsymmetricCipherKeyPair _keyPair;
        readonly Encoding _encoding = Encoding.UTF8;

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
            if (HashBytes == null)
                GenHashPrivKey(enterTextH);

            var text = EncryptRSA.EncryptTextBytes(HashBytes, _keyPair, EKeyEff.Private);
            EncryptedText = text;
            EncryptHash = GetHexModString.GetHexModToString(text);
            return EncryptHash;
        }

        public string DecryptTextHash()
        {
            return DecryptTextHash(EncryptedText);
        }

        public string DecryptTextHash(byte[] encryptedText)
        {
            EncryptedText = encryptedText;
            var nEncrypt = DecryptRSA.DecryptRsaToByte(EncryptedText, _keyPair, EKeyEff.Public);
            if (nEncrypt == null) return null;

            DecryptHash = _encoding.GetString(nEncrypt);
            return GetHexModString.GetHexModToString(nEncrypt);
        }

        public string VerifySignature(string hash, string signature)
        {
            string decryptedHash;
            try
            {
                decryptedHash = signature == hash
                    ? signature
                    : DecryptTextHash(GetHexModString.GetStringToHexMod(signature));
            }
            catch (ArgumentException)
            {
                decryptedHash = null;
            }

            return hash == decryptedHash ? "Pass" : "Hazard!!!";
        }
    }
}
