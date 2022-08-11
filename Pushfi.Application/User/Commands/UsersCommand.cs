using MediatR;
using Pushfi.Application.Common.Models.User;

namespace Pushfi.Application.User.Commands
{
    public class UsersCommand : IRequest<UsersResponseModel>
    {
    }
}
