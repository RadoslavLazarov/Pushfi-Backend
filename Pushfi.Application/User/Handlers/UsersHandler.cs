using MediatR;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.User;
using Pushfi.Application.User.Commands;

namespace Pushfi.Application.User.Handlers
{
    public class UsersHandler : IRequestHandler<UsersCommand, UsersResponseModel>
    {
        private readonly IUserService _userService;

        public UsersHandler(IUserService userService)
        {
            this._userService = userService;
        }

        public async Task<UsersResponseModel> Handle(UsersCommand request, CancellationToken cancellationToken)
        {
            return new UsersResponseModel() { Users = await this._userService.GetAllAsync() };
        }
    }
}
