using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Domain.Configuration;

namespace Pushfi.Infrastructure.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtConfiguration _jwtConfiguration;

        public JwtMiddleware(RequestDelegate next, IOptionsMonitor<JwtConfiguration> optionsMonitor)
        {
            this._next = next;
            this._jwtConfiguration = optionsMonitor.CurrentValue;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtService jwtService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtService.ValidateJwtToken(token);

            if (!string.IsNullOrEmpty(userId))
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userService.GetById(userId);
            }

            await _next(context);
        }
    }
}