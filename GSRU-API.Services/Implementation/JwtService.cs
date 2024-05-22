using GSRU_API.Common.Encryption.Interfaces;
using GSRU_API.Common.Models.Authorize;
using GSRU_API.Common.Settings;
using GSRU_API.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GSRU_API.Services.Implementation
{
    public class JwtService(IOptions<AppSettings> _appSettings, IEncryptionService _encryptionService) : IJwtService
    {
        private readonly AppSettings _appSettings = _appSettings.Value;
        private readonly IEncryptionService _encryptionService = _encryptionService;

        public const string RELOAD_JWT_ROLE = "RELOAD-JWT";
        public AuthenticationResponse CreateJwt(Claim[] claim, string username)
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo utcPlus2TimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time");
            DateTime utcPlus2Now = TimeZoneInfo.ConvertTimeFromUtc(utcNow, utcPlus2TimeZone);


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_encryptionService.Decrypt(_appSettings.JWT.Secret)));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _appSettings.JWT.Issuer,
                _appSettings.JWT.Audience,
                claim,
                notBefore: utcPlus2Now,
                expires: utcPlus2Now.AddMinutes(_appSettings.JWT.ExpireMinutes),
                signingCredentials: credentials
            );
            var hash = claim.AsEnumerable().FirstOrDefault(c => c.Type == ClaimTypes.Hash)?.Value;
            var refreshToken = new JwtSecurityToken(
               _appSettings.JWT.Issuer,
               _appSettings.JWT.Audience,
               GetRefreshClaims(username, hash ?? ""),
               notBefore: utcPlus2Now,
               expires: utcPlus2Now.AddMinutes(_appSettings.JWT.RefreshExpireMinutes),
               signingCredentials: credentials
           );

            return new AuthenticationResponse(token, refreshToken, username);
        }

        public Claim[] GetRefreshClaims(string username, string id)
        {
            return
            [
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, RELOAD_JWT_ROLE),
                new Claim(ClaimTypes.Hash, id)
            ];
        }
    }
}
