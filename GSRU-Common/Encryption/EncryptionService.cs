using GSRU_Common.Encryption.Interfaces;
using GSRU_Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Sodium;
using System.Text;

namespace GSRU_Common.Encryption
{
    public class EncryptionService([FromKeyedServices(ServicesExtensions.EncryptionKey)] byte[] _key, [FromKeyedServices(ServicesExtensions.EncryptionIV)] byte[] _iv) : IEncryptionService
    {
        private readonly byte[] _key = _key;
        private readonly byte[] _iv = _iv;

        public string Encrypt(string plainText)
        {
            var message = Encoding.UTF8.GetBytes(plainText);
            var ciphertext = SecretAeadXChaCha20Poly1305.Encrypt(message, _iv, _key);

            return Convert.ToBase64String(ciphertext);
        }
    }
}
