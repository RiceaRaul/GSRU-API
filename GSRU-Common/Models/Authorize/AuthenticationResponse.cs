using System.IdentityModel.Tokens.Jwt;

namespace GSRU_API.Common.Models.Authorize
{
    public class AuthenticationResponse : GenericError<string>
    {
        public string Token { get; set; } = string.Empty;
        public string TokenType { get; set; } = string.Empty;
        public double ExpiresIn { get; set; }
        public string Username { get; set; } = string.Empty;

        public string RefreshToken { get; set; } = string.Empty;
        public double RefreshExpiresIn { get; set; }

        public AuthenticationResponse() { }

        public AuthenticationResponse(JwtSecurityToken securityToken, JwtSecurityToken refreshToken, string username)
        {
            Token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            TokenType = "Bearer";
            ExpiresIn = securityToken.ValidTo.Subtract(securityToken.ValidFrom).TotalSeconds;
            Username = username;
            RefreshToken = new JwtSecurityTokenHandler().WriteToken(refreshToken);
            RefreshExpiresIn = refreshToken.ValidTo.Subtract(refreshToken.ValidFrom).TotalSeconds;
        }
    }
}
