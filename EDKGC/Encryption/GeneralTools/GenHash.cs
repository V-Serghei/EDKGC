using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EDKGC.Encryption.GeneralTools
{
    public class GenHash
    {
        public static byte[]  GenHashText(string enterText)
        {
            using (SHA256 algorithm = SHA256.Create())
            {
                byte[] hashBytes = algorithm.ComputeHash(Encoding.Default.GetBytes(enterText));
                return hashBytes;
            }
        }
    }
}
