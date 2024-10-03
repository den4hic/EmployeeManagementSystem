using Application.Abstractions;
using Application.Common;
using Application.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Cryptography;

public class AccountService : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService _jwtTokenService;

    public AccountService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<bool>> RegisterAsync(RegisterDto model)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null || await _userManager.FindByNameAsync(model.Username) != null)
            {
                return Result<bool>.Failure("User with this email or username already exists");
            }

            var user = new IdentityUser { UserName = model.Username, Email = model.Email, PhoneNumber = model.PhoneNumber };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "User");
            if (!roleResult.Succeeded)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }

            var userDto = _mapper.Map<UserDto>(model);
            userDto.AspNetUserId = user.Id;
            var userDtoResult = await CreateUserAsync(userDto);

            if (!userDtoResult.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure(userDtoResult.Error);
            }

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

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        return Result<bool>.Success(true);
    }

    public async Task<Result<TokenDto>> GenerateJwtTokenAsync(LoginDto model, bool populateExp)
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
            var accessToken = _jwtTokenService.CreateJwtToken(claims);
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
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Claims.FirstOrDefault(c => c.Type == "username")?.Value);

            if (user is null)
            {
                return Result<TokenDto>.Failure("Invalid token");
            }

            var userDto = await _userRepository.GetByAspNetUserIdAsync(user.Id);

            if (userDto.RefreshToken != tokenDto.RefreshToken || userDto.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return Result<TokenDto>.Failure("Invalid token");
            }

            return await GenerateJwtTokenAsync(new LoginDto { Username = user.UserName, Password = "" }, false);
        }
        catch (Exception ex)
        {
            return Result<TokenDto>.Failure($"Token refresh failed: {ex.Message}");
        }
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

            var addAdmin = await CreateAdminInRepositoryAsync(newAdmin, username, email);
            if (!addAdmin.IsSuccess)
            {
                await _unitOfWork.RollbackAsync();
                return Result<bool>.Failure(addAdmin.Error);
            }

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

    private async Task<Result<bool>> CreateAdminInRepositoryAsync(IdentityUser admin, string username, string email)
    {
        var userDto = new UserDto
        {
            FirstName = username,
            LastName = username,
            Email = email,
            PhoneNumber = "1234567890",
            AspNetUserId = admin.Id
        };

        return await CreateUserAsync(userDto);
    }

    private async Task<Result<bool>> CreateUserAsync(UserDto userDto)
    {
        try
        {
            await _userRepository.CreateAsync(userDto);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Failed to create user: {ex.Message}");
        }
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
}
