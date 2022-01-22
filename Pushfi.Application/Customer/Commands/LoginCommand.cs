using MediatR;
using Pushfi.Application.Common.Models.Authentication;

namespace Pushfi.Application.Customer.Commands
{
	public class LoginCommand : LoginRequestModel, IRequest<LoginResponseModel>
	{
	}
}
