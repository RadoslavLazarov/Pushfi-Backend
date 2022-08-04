using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Customer.Commands;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Exceptions;
using Pushfi.Domain.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pushfi.Application.Customer.Handlers
{
    public class MeHandler : IRequestHandler<MeCommand, MeResponseModel>
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public MeHandler(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async Task<MeResponseModel> Handle(MeCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new BusinessException(string.Format(Strings.InvalidToken));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new BusinessException(string.Format(Strings.InvalidToken));
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return new MeResponseModel()
            {
                Id = userId,
                Email = user.Email,
                Role = userRoles[0]
            };
        }
    }
}
