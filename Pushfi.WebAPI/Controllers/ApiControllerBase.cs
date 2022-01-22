using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pushfi.Application.Common.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace Pushfi.WebAPI.Controllers
{
	[ApiController]
	[Route(RoutingConstants.ApiController)]
	public abstract class ApiControllerBase : ControllerBase
	{
		private ISender _mediator;

		protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
	}
}
