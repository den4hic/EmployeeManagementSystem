using Application.Abstractions;
using Application.Common;
using Application.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _config = config;
    }

    public async Task<Result<bool>> RegisterAsync(RegisterDto model)
    {
        if (await _userManager.FindByEmailAsync(model.Email) != null || await _userManager.FindByNameAsync(model.Username) != null)
        {
            return Result<bool>.Failure("User with this email or username already exists");
        }

        var user = new IdentityUser { UserName = model.Username, Email = model.Email, PhoneNumber = model.PhoneNumber };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            await CreateUserInRepositoryAsync(user, model);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Result<bool>.Success(true);
        }

        return Result<bool>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task<Result<bool>> LoginAsync(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
            return Result<bool>.Failure("Invalid username or password");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
        {
            return Result<bool>.Failure("Invalid username or password");
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> CreateDefaultAdminAsync()
    {
        const string adminUsername = "admin";
        const string adminEmail = "admin@example.com";
        const string adminPassword = "Admin@123";

        var adminUser = await _userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            return await CreateAdminUserAsync(adminUsername, adminEmail, adminPassword);
        }
        return Result<bool>.Success(true);
    }

    public async Task<Result<TokenDto>> GenereateJwtTokenAsync(LoginDto model, bool populateExp)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user is null)
            {
                return Result<TokenDto>.Failure("Invalid user");
            }

            var userDto = await _userRepository.GetByAspNetUserIdAsync(user.Id);

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            if (role is null)
            {
                return Result<TokenDto>.Failure("User has no role");
            }

            var claims = CreateClaims(user, role, userDto.IsBlocked);
            var accessToken = CreateJwtToken(claims);
            var refreshToken = await UpdateUserRefreshTokenAsync(user, populateExp);

            return Result<TokenDto>.Success(new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken });
        }
        catch (Exception ex)
        {
            return Result<TokenDto>.Failure($"Token generation failed: {ex.Message}");
        }
    }

    public async Task<Result<TokenDto>> RefreshToken(TokenDto tokenDto)
    {
        try
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Claims.ToList()[1].Value);

            if (user is null)
            {
                return Result<TokenDto>.Failure("Invalid token");
            }

            var userDto = await _userRepository.GetByAspNetUserIdAsync(user.Id);

            if (userDto.RefreshToken != tokenDto.RefreshToken || userDto.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return Result<TokenDto>.Failure("Invalid token");
            }

            return await GenereateJwtTokenAsync(new LoginDto { Username = user.UserName, Password = "" }, false);
        }
        catch (Exception ex)
        {
            return Result<TokenDto>.Failure($"Token refresh failed: {ex.Message}");
        }
    }


    private async Task CreateUserInRepositoryAsync(IdentityUser user, RegisterDto model)
    {
        var userDto = _mapper.Map<UserDto>(model);
        userDto.AspNetUserId = user.Id;
        await _userRepository.CreateAsync(userDto);
    }

    private async Task<Result<bool>> CreateAdminUserAsync(string username, string email, string password)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var newAdmin = new IdentityUser { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(newAdmin, password);

            if (!result.Succeeded)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure($"Failed to create admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
                
            var addRole = await _userManager.AddToRoleAsync(newAdmin, "Admin");

            if (!addRole.Succeeded)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure("Failed to add role to admin");
            
            }

            await CreateAdminInRepositoryAsync(newAdmin, username, email);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return Result<bool>.Success(true);
        } 
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result<bool>.Failure($"Transaction failed: {ex.Message}");
        }
    }

    private async Task CreateAdminInRepositoryAsync(IdentityUser admin, string username, string email)
    {
        var userDto = new UserDto
        {
            FirstName = username,
            LastName = username,
            Email = email,
            PhoneNumber = "1234567890",
            AspNetUserId = admin.Id
        };

        await _userRepository.CreateAsync(userDto);
    }

    private IEnumerable<Claim> CreateClaims(IdentityUser user, string role, bool isBlocked)
    {
        return new List<Claim>
        {
            new Claim("userData", user.UserName),
            new Claim("username", user.UserName),
            new Claim("role", role),
            new Claim("isBlocked", isBlocked.ToString())
        };
    }

    private string CreateJwtToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config.GetSection("Jwt:ExpireInMinutes").Value)),
            issuer: _config.GetSection("Jwt:Issuer").Value,
            audience: _config.GetSection("Jwt:Audience").Value,
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    private async Task<string> UpdateUserRefreshTokenAsync(IdentityUser user, bool populateExp)
    {
        var userDto = await _userRepository.GetByAspNetUserIdAsync(user.Id);
        var refreshToken = GenerateRefreshToken();

        userDto.RefreshToken = refreshToken;

        if (populateExp)
        {
            userDto.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        }

        await _userRepository.UpdateAsync(userDto);

        return refreshToken;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value)),
            ValidateLifetime = true,
            ValidIssuer = _config.GetSection("Jwt:Issuer").Value,
            ValidAudience = _config.GetSection("Jwt:Audience").Value,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}