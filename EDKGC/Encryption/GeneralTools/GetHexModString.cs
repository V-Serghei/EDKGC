using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDKGC.Encryption.GeneralTools
{
    public class GetHexModString
    {

        public static string GetHexModToString(byte[] byteM)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteM)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        public static byte[] GetStringToHexMod(string hexString)
        {
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}
