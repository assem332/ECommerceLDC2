using ECommerce.Application.DTOs.Users;
using ECommerce.Application.Interfaces.UniteOfWork;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using System;
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

        public AuthService (IUnitOfWork unitOfWork)=> _unitOfWork = unitOfWork;

        public async Task<UserResponseDto> RegisterAsync(UserRegisterDto dto)
        {
            var existingUser = await _unitOfWork.Users.GetByEmailAsync (dto.Email);
            if (existingUser != null) 
                throw new Exception("Email already registered ");

            var hashedPassword = HashPassword(dto.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = hashedPassword,
                Role = UserRole.Customer

            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString ()
            };
        }

        
        public async Task<UserResponseDto> LoginAsync(UserLoginDto dto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid email or password.");

            
            var hashedPassword = HashPassword(dto.Password);
            if (user.PasswordHash != hashedPassword)
                throw new Exception("Invalid email or password.");

            
            return new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToString ()
            };
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

