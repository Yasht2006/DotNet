using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Domain.Models;

namespace UserAuth.Infrastructure.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
