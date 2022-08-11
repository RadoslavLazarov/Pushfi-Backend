using MediatR;
using Pushfi.Application.Common.Models.Authentication;

namespace Pushfi.Application.Authentication.Commands
{
    public class GetRefreshTokensCommand : IRequest<GetRefreshTokensResponseModel>
    {
        public string Id { get; set; }
    }
}
