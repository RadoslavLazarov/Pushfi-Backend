using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pushfi.Application.Common.Models;
using Pushfi.Application.Customer.Commands;

namespace Pushfi.WebAPI.Controllers
{
    public class OfferController : ApiControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route(nameof(SendOffer))]
        public async Task SendOffer([FromQuery] SendOfferCommand command)
        {
            await Mediator.Send(command);
        }

        [Authorize]
        [HttpGet]
        [Route(nameof(GetLatestOffer))]
        public async Task<ActionResult<LatestOfferResponseModel>> GetLatestOffer([FromQuery] LatestOfferCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
