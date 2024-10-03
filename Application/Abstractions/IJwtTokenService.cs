using System.Security.Claims;

namespace Application.Abstractions;

public interface IJwtTokenService
{
    string CreateJwtToken(IEnumerable<Claim> claims);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}

