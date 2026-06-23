namespace EDKGC.Models.AsymmetricAlgorithms
{
    public interface ISharedSecretAsymmetricalAlModel
    {
        byte[] EncryptText(string plaintext);
        string DecryptText(byte[] encryptedBytes);
    }
}
