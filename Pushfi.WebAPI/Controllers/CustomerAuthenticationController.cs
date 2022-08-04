using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pushfi.Application.Common.Models;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Customer.Commands;

namespace Pushfi.WebAPI.Controllers
{
	public class CustomerAuthenticationController : ApiControllerBase
	{
		[HttpPost]
		[Route("{BrokerPath}/" + nameof(Registration))]
		public async Task<ActionResult<RegistrationResponseModel>> Registration(RegistrationCommand command, [FromRoute] string brokerPath)
		{
			command.BrokerPath = brokerPath;
			return await Mediator.Send(command);
		}

		// TODO: change endpoint to CustomerLogin
		[HttpPost]
		[Route("{BrokerPath}/" + nameof(Login))]
		public async Task<ActionResult<LoginResponseModel>> Login(LoginCommand command, [FromRoute] string brokerPath)
		{
			command.BrokerPath = brokerPath;
			return await Mediator.Send(command);
		}

		[Authorize]
		[HttpDelete]
		[Route(nameof(Delete))]
		public async Task Delete([FromQuery] DeleteCommand command)
		{
			await Mediator.Send(command);
		}

		[Authorize]
		[HttpGet]
        [Route(nameof(ProcessStatus) + "/{BrokerPath}")]
        public async Task<ActionResult<ProcessStatusModel>> ProcessStatus([FromQuery] ProcessStatusCommand command, [FromRoute] string brokerPath)
        {
			command.BrokerPath= brokerPath;
            return await Mediator.Send(command);
        }
	}
}
