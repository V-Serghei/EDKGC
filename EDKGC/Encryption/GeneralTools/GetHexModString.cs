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
    }
}
