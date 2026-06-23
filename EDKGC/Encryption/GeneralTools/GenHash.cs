using System.Security.Cryptography;
using System.Text;

namespace EDKGC.Encryption.GeneralTools
{
    public class GenHash
    {
        public static byte[] GenHashText(string enterText)
        {
            using (SHA256 algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(enterText));
            }
        }
    }
}
