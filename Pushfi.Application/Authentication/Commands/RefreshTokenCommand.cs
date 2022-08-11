using MediatR;
using Pushfi.Application.Common.Models.Authentication;

namespace Pushfi.Application.Authentication.Commands
{
    public class RefreshTokenCommand : IRequest<AuthenticateResponseModel>
    {
        public string RefreshToken { get; set; }

        public string IpAddress { get; set; }
    }
}
