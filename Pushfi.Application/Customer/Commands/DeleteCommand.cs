using MediatR;
using Pushfi.Application.Common.Models;

namespace Pushfi.Application.Customer.Commands
{
    public class DeleteCommand : IRequest<ResponseModel>
    {
        public Guid UserId { get; set; }
    }
}
