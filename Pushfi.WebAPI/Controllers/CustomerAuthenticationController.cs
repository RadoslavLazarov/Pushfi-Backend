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
		[Route(nameof(Registration))]
		public async Task<ActionResult<RegistrationResponseModel>> Registration(RegistrationCommand command)
		{
			return await Mediator.Send(command);
		}

		[HttpPost]
		[Route(nameof(Login))]
		public async Task<ActionResult<LoginResponseModel>> Login(LoginCommand command)
		{
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
        [Route(nameof(ProcessStatus))]
        public async Task<ActionResult<ProcessStatusModel>> ProcessStatus([FromQuery] ProcessStatusCommand command)
        {
            return await Mediator.Send(command);
        }
	}
}
