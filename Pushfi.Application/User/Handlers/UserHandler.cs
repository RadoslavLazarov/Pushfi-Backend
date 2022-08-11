using MediatR;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Common.Models.User;
using Pushfi.Application.User.Commands;

namespace Pushfi.Application.User.Handlers
{
    public class UserHandler : IRequestHandler<UserCommand, UserModel>
    {
        private readonly IUserService _userService;

        public UserHandler(IUserService userService)
        {
            this._userService = userService;
        }

        public async Task<UserModel> Handle(UserCommand request, CancellationToken cancellationToken)
        {
            return this._userService.GetById(request.Id);
        }
    }
}
