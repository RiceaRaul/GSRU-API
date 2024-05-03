﻿using GSRU_API.Common.Encryption;
using GSRU_API.Common.Encryption.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GSRU_Common.Extensions
{
    public static class ServicesExtensions
    {
        public const string EncryptionKey = "GSRU__ENCRYPTION__KEY";
        public const string EncryptionIV = "GSRU__ENCRYPTION__IV";
        public static void AddEncryptionService(this IServiceCollection services)
        {
            string? env_key = Environment.GetEnvironmentVariable(EncryptionKey);
            ArgumentNullException.ThrowIfNull(env_key);
            byte[] key = Convert.FromBase64String(env_key);
            string? env_iv = Environment.GetEnvironmentVariable(EncryptionIV);
            ArgumentNullException.ThrowIfNull(env_iv);
            byte[] iv = Convert.FromBase64String(env_iv);

            services.AddSingleton<IEncryptionService, EncryptionService>(provider => new EncryptionService(key,iv));
        }
    }
}