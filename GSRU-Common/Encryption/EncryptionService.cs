using GSRU_API.Common.Encryption.Interfaces;
using Sodium;
using System.Text;

namespace GSRU_API.Common.Encryption
{
    public class EncryptionService(byte[] _key, byte[] _iv) : IEncryptionService
    {
        private readonly byte[] _key = _key;
        private readonly byte[] _iv = _iv;

        public string Encrypt(string plainText)
        {
            var message = Encoding.UTF8.GetBytes(plainText);
            var ciphertext = SecretAeadXChaCha20Poly1305.Encrypt(message, _iv, _key);
            var encryptedResult = Convert.ToBase64String(ciphertext);

            return encryptedResult;
        }

        public string Decrypt(string plainText)
        {

            var message = Convert.FromBase64String(plainText);
            var ciphertext = SecretAeadXChaCha20Poly1305.Decrypt(message, _iv, _key);

            return Encoding.UTF8.GetString(ciphertext);
        }
    }
}
