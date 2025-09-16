using UserAuth.Infrastructure.Services.Interfaces;

namespace UserAuth.Middleware
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenBlacklistMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITokenBlacklistService blacklistService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            if (!string.IsNullOrWhiteSpace(token))
            {
                var isBlacklisted = await blacklistService.IsTokenBlacklistedAsync(token);
                if (isBlacklisted)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Token has been blacklisted. Please log in again.");
                    return;
                }
            }

            await _next(context);
        }
    }
}
