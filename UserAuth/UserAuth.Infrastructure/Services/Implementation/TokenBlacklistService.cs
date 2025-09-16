using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Domain.Models;
using UserAuth.Infrastructure.Context;
using UserAuth.Infrastructure.Repositories.Interfaces;
using UserAuth.Infrastructure.Services.Interfaces;

namespace UserAuth.Infrastructure.Services.Implementation
{
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly IGenericRepository<BlacklistedToken> _blackRepo;

        public TokenBlacklistService(IGenericRepository<BlacklistedToken> blackRepo)
        {
            _blackRepo = blackRepo;
        }

        public async Task BlacklistTokenAsync(string token, DateTime expiration)
        {
            var blacklistedToken = new BlacklistedToken
            {
                Token = token,
                Expiration = expiration
            };

            await _blackRepo.AddAsync(blacklistedToken);
            await _blackRepo.SaveChangesAsync();


        }
        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return await _blackRepo.AnyAsync(bt => bt.Token == token && bt.Expiration > DateTime.UtcNow);
        }
    }
}
