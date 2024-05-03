namespace GSRU_API.Common.Encryption.Interfaces
{
    public interface IEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string plainText);
    }
}
