using MediatR;
using Pushfi.Application.Common.Models.Authentication;

namespace Pushfi.Application.User.Commands
{
    public class UserCommand : IRequest<UserModel>
    {
        public string Id { get; set; }
    }
}
