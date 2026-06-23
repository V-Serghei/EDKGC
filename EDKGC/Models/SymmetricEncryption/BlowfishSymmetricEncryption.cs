namespace EDKGC.Models.SymmetricEncryption
{
    public class BlowfishSymmetricEncryption : BouncyBlockSymmetricEncryption
    {
        protected override int KeyLength => 16;

        protected override Org.BouncyCastle.Crypto.IBlockCipher CreateEngine()
        {
            return new Org.BouncyCastle.Crypto.Engines.BlowfishEngine();
        }
    }
}
