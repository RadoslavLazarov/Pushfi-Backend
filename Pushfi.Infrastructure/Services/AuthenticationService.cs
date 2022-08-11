using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Domain.Configuration;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Entities.Broker;
using Pushfi.Domain.Entities.Customer;
using Pushfi.Domain.Exceptions;
using Pushfi.Domain.Resources;
using System.Security;
using System.Security.Claims;

namespace Pushfi.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IEnfortraService _enfortraService;

        public AuthenticationService(
            IHttpContextAccessor accessor,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IApplicationDbContext context,
            IJwtService jwtService,
            IOptionsMonitor<JwtConfiguration> optionsMonitor,
            IEnfortraService enfortraService)
        {
            this._accessor = accessor;
            this._mapper = mapper;
            this._userManager = userManager;
            this._context = context;
            this._jwtService = jwtService;
            this._jwtConfiguration = optionsMonitor.CurrentValue;
            this._enfortraService = enfortraService;
        }

        public void RemoveOldRefreshTokens(ApplicationUser user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            var tokens = user.RefreshTokens
                .Where(x =>
                    !x.IsActive &&
                    x.CreatedAt.AddDays(this._jwtConfiguration.RefreshTokenTTL) <= DateTime.UtcNow)
                .ToList();

            this._context.DbContext.RemoveRange(tokens);
            this._context.DbContext.SaveChanges();
        }

        public ApplicationUser GetUserByRefreshToken(string token)
        {
            var user = this._userManager.Users
                .Include(x => x.RefreshTokens)
                .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                throw new SecurityException("Invalid token");
            }

            return user;
        }

        public void RevokeDescendantRefreshTokens(RefreshTokenEntity refreshToken, ApplicationUser user, string ipAddress, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive)
                {
                    this.RevokeRefreshToken(childToken, ipAddress, reason);
                }
                else
                {
                    this.RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
                }
            }
        }

        public RefreshTokenEntity RotateRefreshToken(RefreshTokenEntity refreshToken, string ipAddress)
        {
            var newRefreshToken = this._jwtService.GenerateRefreshToken(ipAddress);
            this.RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        public void RevokeRefreshToken(RefreshTokenEntity token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }
    }
}
