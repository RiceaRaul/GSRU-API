using GSRU_API.Common.Models.Authorize;
using System.Security.Claims;

namespace GSRU_API.Services.Interfaces
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwt(Claim[] claim, string username);
    }
}
