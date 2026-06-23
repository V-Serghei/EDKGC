namespace EDKGC.Models.SymmetricEncryption
{
    public class TwofishSymmetricEncryption : BouncyBlockSymmetricEncryption
    {
        protected override int KeyLength => 32;

        protected override Org.BouncyCastle.Crypto.IBlockCipher CreateEngine()
        {
            return new Org.BouncyCastle.Crypto.Engines.TwofishEngine();
        }
    }
}
