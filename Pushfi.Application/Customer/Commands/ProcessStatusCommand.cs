using MediatR;
using Pushfi.Application.Common.Models.Authentication;

namespace Pushfi.Application.Customer.Commands
{
    public class ProcessStatusCommand : IRequest<ProcessStatusModel>
    {
        public string BrokerPath { get; set; }
    }
}
