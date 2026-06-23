using System;
using System.IO;
using System.Text;
using EDKGC.Encryption.GeneralTools;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace EDKGC.Models.AsymmetricAlgorithms
{
    public class ElGamalAsymmetricalAlModel
    {
        private AsymmetricCipherKeyPair _keyPair;

        public byte[] EncryptedText { get; set; }

        public void GenerateKeys()
        {
            var random = new SecureRandom();
            var parametersGenerator = new ElGamalParametersGenerator();
            parametersGenerator.Init(512, 20, random);
            var parameters = parametersGenerator.GenerateParameters();

            var keyPairGenerator = new ElGamalKeyPairGenerator();
            keyPairGenerator.Init(new ElGamalKeyGenerationParameters(random, parameters));
            _keyPair = keyPairGenerator.GenerateKeyPair();
        }

        public byte[] GetPublicKey()
        {
            return ((ElGamalPublicKeyParameters)_keyPair.Public).Y.ToByteArrayUnsigned();
        }

        public byte[] GetPrivateKey()
        {
            return ((ElGamalPrivateKeyParameters)_keyPair.Private).X.ToByteArrayUnsigned();
        }

        public byte[] EncryptText(string plaintext)
        {
            if (_keyPair == null) GenerateKeys();
            var input = Encoding.UTF8.GetBytes(plaintext);
            EncryptedText = ProcessBlocks(input, _keyPair.Public, true);
            return EncryptedText;
        }

        public string DecryptText(byte[] encryptedBytes)
        {
            var decrypted = ProcessBlocks(encryptedBytes, _keyPair.Private, false);
            return Encoding.UTF8.GetString(decrypted);
        }

        private static byte[] ProcessBlocks(byte[] input, ICipherParameters key, bool forEncryption)
        {
            var engine = new ElGamalEngine();
            engine.Init(forEncryption, key);
            int inputBlockSize = engine.GetInputBlockSize();
            int outputBlockSize = engine.GetOutputBlockSize();

            using (var output = new MemoryStream())
            {
                if (forEncryption)
                {
                    for (int offset = 0; offset < input.Length; offset += inputBlockSize)
                    {
                        int length = Math.Min(inputBlockSize, input.Length - offset);
                        byte[] block = engine.ProcessBlock(input, offset, length);
                        output.Write(block, 0, block.Length);
                    }
                }
                else
                {
                    for (int offset = 0; offset < input.Length; offset += inputBlockSize)
                    {
                        int length = Math.Min(inputBlockSize, input.Length - offset);
                        byte[] block = engine.ProcessBlock(input, offset, length);
                        output.Write(block, 0, block.Length);
                    }
                }

                return output.ToArray();
            }
        }
    }
}
