using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAuth.Infrastructure.Services.Interfaces
{
    public interface ITokenBlacklistService
    {
        Task BlacklistTokenAsync(string token, DateTime expiration);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }
}
