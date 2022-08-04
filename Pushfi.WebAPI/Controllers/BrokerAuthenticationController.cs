using Microsoft.AspNetCore.Mvc;
using Pushfi.Application.Broker.Commands;

namespace Pushfi.WebAPI.Controllers
{
    public class BrokerAuthenticationController : ApiControllerBase
    {
		[HttpPost]
        [Route(nameof(Registration))]
        //[DisableRequestSizeLimit]
        public async Task Registration([FromForm] BrokerRegistrationCommand command)
        {
			await Mediator.Send(command);
        }
	}
}
