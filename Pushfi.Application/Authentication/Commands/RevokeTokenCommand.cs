using MediatR;
using Pushfi.Application.Common.Models.Authentication;

namespace Pushfi.Application.Authentication.Commands
{
    public class RevokeTokenCommand : IRequest<RevokeTokenResponseModel>
    {
        public string Token { get; set; }

        public string IpAddress { get; set; }
    }
}
