﻿using Application.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions;

public interface IAccountService
{
    Task<bool> RegisterAsync(RegisterDto model);
    Task<bool> LoginAsync(LoginDto model);
    Task LogoutAsync();
    Task<bool> CreateDefaultAdminAsync();
    Task<TokenDto> GenereateJwtTokenAsync(LoginDto model, bool populateExp);
    Task<TokenDto> RefreshToken(TokenDto tokenDto);
}