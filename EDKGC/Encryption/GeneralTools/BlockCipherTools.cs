using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace EDKGC.Encryption.GeneralTools
{
    public static class BlockCipherTools
    {
        public static byte[] EncryptEbc(byte[] enterText, byte[] key, IBlockCipher engine)
        {
            return ProcessCipher(enterText, key, engine, true);
        }

        public static byte[] DecryptEbc(byte[] encryptedText, byte[] key, IBlockCipher engine)
        {
            return ProcessCipher(encryptedText, key, engine, false);
        }

        private static byte[] ProcessCipher(byte[] input, byte[] key, IBlockCipher engine, bool forEncryption)
        {
            var cipher = new PaddedBufferedBlockCipher(new EcbBlockCipher(engine), new Pkcs7Padding());
            cipher.Init(forEncryption, new KeyParameter(key));

            byte[] output = new byte[cipher.GetOutputSize(input.Length)];
            int length = cipher.ProcessBytes(input, 0, input.Length, output, 0);
            length += cipher.DoFinal(output, length);

            if (length == output.Length) return output;

            byte[] result = new byte[length];
            System.Array.Copy(output, result, length);
            return result;
        }
    }
}
