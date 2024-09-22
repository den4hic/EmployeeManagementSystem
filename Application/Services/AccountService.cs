using Application.Abstractions;
using Application.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public AccountService(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserRepository userRepository, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _mapper = mapper;
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
    }
}
