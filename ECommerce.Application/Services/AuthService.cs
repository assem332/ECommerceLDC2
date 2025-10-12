    using ECommerce.Application.DTOs.Users;
    using ECommerce.Domain.Entities;
    using System;
    using ECommerce.Application.Interfaces.UniteOfWork; 
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Security.Cryptography;

    namespace ECommerce.Application.Services
    {
        public class AuthService
        {

            private readonly IUnitOfWork _unitOfWork;
            private readonly JwtTokenService _jwtTokenService;

            public AuthService(IUnitOfWork unitOfWork, JwtTokenService jwtTokenService)
            {
                _unitOfWork = unitOfWork;
                _jwtTokenService = jwtTokenService;
            }

       
            public async Task<UserResponseDto> RegisterAsync(UserRegisterDto dto)
            {
                var existingUser = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
                if (existingUser != null)
                    throw new Exception("Email already registered.");

                var hashedPassword = HashPassword(dto.Password);

                var user = new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    PasswordHash = hashedPassword,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address,
                    IsAdmin = dto.IsAdmin
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.CompleteAsync();

                return new UserResponseDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    IsAdmin = user.IsAdmin
                };
            }

        
            public async Task<(UserResponseDto User, string Token)> LoginAsync(UserLoginDto dto)
            {
                var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
                if (user == null)
                    throw new Exception("Invalid email or password.");

                var hashedPassword = HashPassword(dto.Password);
                if (user.PasswordHash != hashedPassword)
                    throw new Exception("Invalid email or password.");

                var token = _jwtTokenService.GenerateToken(user);

                var response = new UserResponseDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    IsAdmin = user.IsAdmin
                };  

                return (response, token);
            }

        
            private string HashPassword(string password)
            {
                using var sha256 = SHA256.Create();
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        }

