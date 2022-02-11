using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
