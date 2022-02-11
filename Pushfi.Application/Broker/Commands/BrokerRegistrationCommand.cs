using MediatR;
using Pushfi.Application.Common.Models.Broker;

namespace Pushfi.Application.Broker.Commands
{
    public class BrokerRegistrationCommand : BrokerModel, IRequest
    {
    }
}
