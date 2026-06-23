namespace EDKGC.Models.SymmetricEncryption
{
    public class TripleDesSymmetricEncryption : BouncyBlockSymmetricEncryption
    {
        protected override int KeyLength => 24;

        protected override Org.BouncyCastle.Crypto.IBlockCipher CreateEngine()
        {
            return new Org.BouncyCastle.Crypto.Engines.DesEdeEngine();
        }
    }
}
