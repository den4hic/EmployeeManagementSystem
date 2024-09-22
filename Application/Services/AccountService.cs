using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
        var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
        if (existingUserByEmail != null)
        {
            return false;
        }

        var existingUserByUsername = await _userManager.FindByNameAsync(model.Username);
        if (existingUserByUsername != null)
        {
            return false;
        }

        var user = new IdentityUser { UserName = model.Username, Email = model.Email, PhoneNumber = model.PhoneNumber };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");

            var userDto = _mapper.Map<UserDto>(model);
            userDto.AspNetUserId = user.Id;
            await _userRepository.CreateAsync(userDto);

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

        var role = await _userManager.GetRolesAsync(user);

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
        string adminUsername = "admin";
        string adminEmail = "admin@example.com";
        string adminPassword = "Admin@123";

        var adminUser = await _userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var newAdmin = new IdentityUser { UserName = adminUsername, Email = adminEmail };
            var result = await _userManager.CreateAsync(newAdmin, adminPassword);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newAdmin, "Admin");

                var userDto = new UserDto
                {
                    FirstName = adminUsername,
                    LastName = adminUsername,
                    Email = adminEmail,
                    PhoneNumber = "1234567890",
                    AspNetUserId = newAdmin.Id
                };

                await _userRepository.CreateAsync(userDto);
                return true;
            }
            return false;
        }
        return true;
    }

    public async Task<TokenDto> GenereateJwtTokenAsync(LoginDto model, bool populateExp)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();

        IEnumerable <Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.UserData, user.UserName),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, role)
        };

        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        SecurityToken securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config.GetSection("Jwt:ExpireInMinutes").Value)),
            issuer: _config.GetSection("Jwt:Issuer").Value,
            audience: _config.GetSection("Jwt:Audience").Value,
            signingCredentials: signingCredentials
            );

        var userDto = await _userRepository.GetByAspNetUserIdAsync(user.Id);

        var refreshToken = GenerateRefreshToken();

        userDto.RefreshToken = refreshToken;

        if (populateExp)
        {
            userDto.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        }

        await _userRepository.UpdateAsync(userDto);

        string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return new TokenDto { AccessToken = tokenString, RefreshToken = refreshToken };
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
        var tokenValidParametrs = new TokenValidationParameters
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
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidParametrs, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
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
}
