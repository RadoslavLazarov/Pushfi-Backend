using Microsoft.AspNetCore.Mvc;
using Pushfi.Application.Common.Models;
using Pushfi.Application.Broker.Commands;

namespace Pushfi.WebAPI.Controllers
{
    public class BrokerController : ApiControllerBase
    {
        [HttpGet]
        [Route("{BrokerPath}")]
        public async Task<ActionResult<BrokerDataForCustomerFormModel>> Get([FromRoute] BrokerDataCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
