using MediatR;
using Pushfi.Application.Common.Models.Authentication;

namespace Pushfi.Application.Customer.Commands
{
    public class GetAllCustomersCommand : IRequest<List<CustomerModel>>
    {
    }
}
