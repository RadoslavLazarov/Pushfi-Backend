using MediatR;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Domain.Models;

namespace Pushfi.Application.Customer.Commands
{
    public class GetAllCustomersCommand : PageModel, IRequest<PageResult<CustomerModel>>
    {
    }
}
