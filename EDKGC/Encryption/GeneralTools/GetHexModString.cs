using System;
using System.Globalization;
using System.Text;

namespace EDKGC.Encryption.GeneralTools
{
    // ReSharper disable LocalizableElement
    public static class GetHexModString
    {
        public static string GetHexModToString(byte[] byteM)
        {
            var sb = new StringBuilder(byteM.Length * 2);
            foreach (byte b in byteM)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public static byte[] GetStringToHexMod(string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
                throw new ArgumentException("Hex string must not be empty.", nameof(hexString));
            if (hexString.Length % 2 != 0)
                throw new ArgumentException("Hex string must have an even number of characters.", nameof(hexString));

            var bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                if (!byte.TryParse(hexString.AsSpan(i, 2), NumberStyles.HexNumber, null, out bytes[i / 2]))
                    throw new ArgumentException($"Invalid hex characters at position {i}.", nameof(hexString));
            }
            return bytes;
        }
    }
}
