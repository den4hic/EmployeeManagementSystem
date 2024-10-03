using Application.Common;
using Application.DTOs;

namespace Application.Abstractions;

public interface IAccountService
{
    Task<Result<bool>> RegisterAsync(RegisterDto model);
    Task<Result<bool>> LoginAsync(LoginDto model);
    Task<Result<bool>> LogoutAsync();
    Task<Result<bool>> CreateDefaultAdminAsync();
    Task<Result<TokenDto>> GenerateJwtTokenAsync(LoginDto model, bool populateExp);
    Task<Result<TokenDto>> RefreshToken(TokenDto tokenDto);
}
