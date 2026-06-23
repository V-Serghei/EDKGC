using System.Security.Cryptography;

namespace EDKGC.Encryption.DES
{
    public class GenerateKeyDes
    {
        public static byte[] GenKeyDes()
        {
            using (var des = System.Security.Cryptography.DES.Create())
            {
                des.GenerateKey();
                return des.Key;
            }
        }

        public static byte[] GenIv()
        {
            byte[] iv = new byte[8];
            RandomNumberGenerator.Fill(iv);
            return iv;
        }
    }
}
