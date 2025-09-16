using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Application.DTOs.User;
using UserAuth.Domain.Models;
using UserAuth.Infrastructure.Repositories.Interfaces;
using UserAuth.Infrastructure.Services.Interfaces;

namespace UserAuth.Infrastructure.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserService(IGenericRepository<User> userRepository, IMapper mapper, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<UserResponseDTO> RegisterAsync(UserCreateDTO dto)
        {
            try
            {
                var existingUser = await _userRepository.FindAsync(u => u.Email == dto.Email);
                if (existingUser != null)
                {
                    throw new Exception("User already exists with this email.");
                }

                var user = _mapper.Map<User>(dto);
                user.Role = "Customer";
                user.CreatedBy = 1;
                user.CreatedOn = DateTime.UtcNow;

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();

                return _mapper.Map<UserResponseDTO>(user);
            }
            catch (Exception)
            {
                throw;

            }
        }

        public async Task<string> LoginAsync(LoginDTO dto)
        {
            try
            {
                var user = await _userRepository.FindAsync(u => u.Email == dto.Email && u.Password == dto.Password);

                if (user == null || user.Password != dto.Password)
                {
                    throw new Exception("Invalid Email or Password!");
                }

                // Generate JWT token
                var token = _tokenService.CreateToken(user);

                // Return user info with token
                return token;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
