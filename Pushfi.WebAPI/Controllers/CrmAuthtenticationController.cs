using Microsoft.AspNetCore.Mvc;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Customer.Commands;

namespace Pushfi.WebAPI.Controllers
{
    public class CrmAuthenticationController : ApiControllerBase
    {
		[HttpPost]
		[Route(nameof(Login))]
		public async Task<ActionResult<CrmLoginResponseModel>> Login(CrmLoginCommand command)
		{
			return await Mediator.Send(command);
		}

		[HttpGet]
		[Route(nameof(Me))]
		public async Task<ActionResult<MeResponseModel>> Me([FromHeader] MeCommand command)
		{
			return await Mediator.Send(command);
		}
	}
}
