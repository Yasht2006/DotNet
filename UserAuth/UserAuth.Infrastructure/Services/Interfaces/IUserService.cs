using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAuth.Application.DTOs.User;

namespace UserAuth.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO> RegisterAsync(UserCreateDTO dto);
        Task<string> LoginAsync(LoginDTO dto);
    }
}
