using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Domain.Models;
using UserAuth.Infrastructure.Services.Interfaces;

namespace UserAuth.Infrastructure.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(User user)
        {
            try
            {
                var claims = new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
        };

                var key = _config["Jwt:Key"];
                if (string.IsNullOrWhiteSpace(key))
                    throw new Exception("JWT Key is missing in configuration.");

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while generating JWT token: " + ex.Message, ex);
            }
        }
    }
}
