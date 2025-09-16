using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using UserAuth.Application.DTOs.User;
using UserAuth.Infrastructure.Context;
using UserAuth.Infrastructure.Services.Interfaces;

namespace UserAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly UserDbContext _context;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, UserDbContext context, IMapper mapper)
        {
            _userService = userService;
            _context = context;
            _mapper = mapper;
        }


        [HttpPost("register")]
        //[AllowAnonymous]
        public async Task<IActionResult> Register(UserCreateDTO dto)
        {
            //var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var user = await _userService.RegisterAsync(dto);

            var message = "You are successfully Register..!";

            return Ok(new { message, user });

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {

            var token = await _userService.LoginAsync(dto);

            var message = "You are successfully logged in..!";

            return Ok(new { message, token });

        }


        [HttpGet]
        [Route("GetAllUsers")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<IActionResult> GetAllUsers()
        {

            var users = _context.Users.ToList();

            if (users.IsNullOrEmpty())
            {
                throw new Exception("User not found..!");
            }
            var usr = _mapper.Map<IEnumerable<UserResponseDTO>>(users);


            return Ok(new { usr });

        }


        [HttpPost("logout")]
        [Authorize(Roles = "Admin, Customer")]
        public async Task<IActionResult> Logout(
    [FromServices] ITokenBlacklistService blacklistService,
    [FromServices] IHttpContextAccessor httpContextAccessor)
        {
            var token = httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
                .FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest(new { message = "Token not found in request" });
            }

            // Get expiration from claims (you must include 'exp' in JWT token)
            var expClaim = User.FindFirst("exp")?.Value;
            var expiration = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim!)).UtcDateTime;

            await blacklistService.BlacklistTokenAsync(token, expiration);

            return Ok(new { message = "You have been logged out successfully." });
        }

    }
}
