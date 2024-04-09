using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EDKGC.Encryption.DES
{
    public class GenerateKeyDes
    {
        public static byte[] GenKeyDes()
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.GenerateKey();
                return des.Key;
            }
        }

        public static byte[] GenIv()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] iv = new byte[8]; 
                rng.GetBytes(iv);
                return iv;
            }

        }

    }
}
