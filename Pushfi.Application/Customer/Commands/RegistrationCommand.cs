using MediatR;
using Pushfi.Application.Common.Models.Authentication;

namespace Pushfi.Application.Customer.Commands
{
	public class RegistrationCommand : CustomerModel, IRequest<RegistrationResponseModel>
	{
		public string BrokerPath { get; set; }
	}
}
