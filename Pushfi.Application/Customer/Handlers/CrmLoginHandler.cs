﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Customer.Commands;
using Pushfi.Domain.Configuration;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Exceptions;
using Pushfi.Domain.Resources;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pushfi.Application.Customer.Handlers
{
    public class CrmLoginHandler : IRequestHandler<CrmLoginCommand, CrmLoginResponseModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IEnfortraService _enfortraService;

        public CrmLoginHandler(
            IApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptionsMonitor<JwtConfiguration> optionsMonitor,
            IEnfortraService enfortraService)
        {
            this._context = context;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._jwtConfiguration = optionsMonitor.CurrentValue;
            this._enfortraService = enfortraService;
        }

        public async Task<CrmLoginResponseModel> Handle(CrmLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new BusinessException(string.Format(Strings.UserDoesNotExsists));
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new BusinessException(Strings.WrongPassword);
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtConfiguration.Secret));

            var token = new JwtSecurityToken(
                issuer: this._jwtConfiguration.ValidIssuer,
                audience: this._jwtConfiguration.ValidAudience,
                expires: DateTime.UtcNow.AddHours(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            // TODO: implement refresh token
            return new CrmLoginResponseModel
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                Role = userRoles[0]
            };
        }
    }
}
