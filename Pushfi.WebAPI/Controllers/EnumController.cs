using Microsoft.AspNetCore.Mvc;
using Pushfi.Application.Enum.Commands;
using Pushfi.Domain.Extensions;

namespace Pushfi.WebAPI.Controllers
{
    public class EnumController : ApiControllerBase
    {
		[HttpGet]
		public async Task<ActionResult<List<EnumNameValue>>> Get([FromQuery] EnumCommand command)
		{
			return await Mediator.Send(command);
		}
	}
}
