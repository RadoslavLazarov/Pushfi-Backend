using AutoMapper;
using MediatR;
using Pushfi.Application.Authentication.Commands;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.Authentication;

namespace Pushfi.Application.Authentication.Handlers
{
    public class GetRefreshTokensHandler : IRequestHandler<GetRefreshTokensCommand, GetRefreshTokensResponseModel>
    {
        private readonly IMapper _mapper;
        private IUserService _userService;

        public GetRefreshTokensHandler(
            IMapper mapper,
            IUserService userService)
        {
            this._mapper = mapper;
            this._userService = userService;
        }

        public async Task<GetRefreshTokensResponseModel> Handle(GetRefreshTokensCommand request, CancellationToken cancellationToken)
        {
            var user = this._userService.GetById(request.Id);
            var tokensModel = new List<RefreshTokenModel>();

            foreach (var token in user.RefreshTokens)
            {
                tokensModel.Add(this._mapper.Map<RefreshTokenModel>(token));
            }

            return new GetRefreshTokensResponseModel() { RefreshTokens = tokensModel };
        }
    }
}
