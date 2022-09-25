using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pushfi.Application.Common.Models;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Customer.Commands;
using Pushfi.Domain.Enums;
using Pushfi.Domain.Models;

namespace Pushfi.WebAPI.Controllers
{
	public class CustomerController : ApiControllerBase
	{
		[Authorize]
		[HttpDelete]
		[Route(nameof(Delete))]
		public async Task Delete([FromQuery] DeleteCurrentCustomerCommand command)
		{
			await Mediator.Send(command);
		}

        [Authorize]
        [HttpDelete]
        [Route(nameof(DeleteByUserId))]
        public async Task<ResponseModel> DeleteByUserId([FromQuery] DeleteCommand command)
        {
            return await Mediator.Send(command);
        }

        [Authorize]
		[HttpGet]
        [Route(nameof(ProcessStatus) + "/{BrokerPath}")]
        public async Task<ActionResult<ProcessStatusModel>> ProcessStatus([FromQuery] ProcessStatusCommand command, [FromRoute] string brokerPath)
        {
			command.BrokerPath= brokerPath;
            return await Mediator.Send(command);
        }

		[Authorize(Roles = "Broker")]
		[HttpGet]
		[Route(nameof(GetBrokerCustomers))]
		public async Task<ActionResult<PageResult<CustomerModel>>> GetBrokerCustomers([FromQuery] GetBrokerCustomersCommand command)
		{
			return await Mediator.Send(command);
		}

		[Authorize(Roles = "Admin")]
		[HttpGet]
		[Route(nameof(GetAll))]
		public async Task<ActionResult<PageResult<CustomerModel>>> GetAll([FromQuery] GetAllCustomersCommand command)
		{
			return await Mediator.Send(command);
		}
	}
}
