using Application.Abstractions;
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
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserRepository userRepository, IMapper mapper, IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
        _mapper = mapper;
        _config = config;
    }

    public async Task<bool> RegisterAsync(RegisterDto model)
    {
        if (await _userManager.FindByEmailAsync(model.Email) != null || await _userManager.FindByNameAsync(model.Username) != null)
        {
            return false;
        }

        var user = new IdentityUser { UserName = model.Username, Email = model.Email, PhoneNumber = model.PhoneNumber };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            await CreateUserInRepositoryAsync(user, model);
            await _signInManager.SignInAsync(user, isPersistent: false);
            return true;
        }

        return false;
    }

    public async Task<bool> LoginAsync(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
            return false;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
        {
            return false;
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return true;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> CreateDefaultAdminAsync()
    {
        const string adminUsername = "admin";
        const string adminEmail = "admin@example.com";
        const string adminPassword = "Admin@123";

        var adminUser = await _userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            return await CreateAdminUserAsync(adminUsername, adminEmail, adminPassword);
        }
        return true;
    }

    public async Task<TokenDto> GenereateJwtTokenAsync(LoginDto model, bool populateExp)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user is null)
        {
            throw new SecurityTokenException("Invalid token");
        }

        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

        if (role is null)
        {
            throw new SecurityTokenException("User has no role");
        }

        var claims = CreateClaims(user, role);
        var accessToken = CreateJwtToken(claims);
        var refreshToken = await UpdateUserRefreshTokenAsync(user, populateExp);

        return new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken };
    }

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        var user = await _userManager.FindByNameAsync(principal.Identity.Name);

        if (user is null)
        {
            throw new SecurityTokenException("Invalid token");
        }

        var userDto = await _userRepository.GetByAspNetUserIdAsync(user.Id);

        if (userDto.RefreshToken != tokenDto.RefreshToken || userDto.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new SecurityTokenException("Invalid token");
        }

        return await GenereateJwtTokenAsync(new LoginDto { Username = user.UserName, Password = "" }, false);
    }

    private async Task CreateUserInRepositoryAsync(IdentityUser user, RegisterDto model)
    {
        var userDto = _mapper.Map<UserDto>(model);
        userDto.AspNetUserId = user.Id;
        await _userRepository.CreateAsync(userDto);
    }

    private async Task<bool> CreateAdminUserAsync(string username, string email, string password)
    {
        var newAdmin = new IdentityUser { UserName = username, Email = email };
        var result = await _userManager.CreateAsync(newAdmin, password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(newAdmin, "Admin");
            await CreateAdminInRepositoryAsync(newAdmin, username, email);
            return true;
        }
        return false;
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

    private IEnumerable<Claim> CreateClaims(IdentityUser user, string role)
    {
        return new List<Claim>
        {
            new Claim("userData", user.UserName),
            new Claim("username", user.UserName),
            new Claim("role", role)
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