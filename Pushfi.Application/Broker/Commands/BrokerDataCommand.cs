using MediatR;
using Pushfi.Application.Common.Models;

namespace Pushfi.Application.Broker.Commands
{
    public class BrokerDataCommand : IRequest<BrokerDataForCustomerFormModel>
    {
        public string BrokerPath { get; set; }
    }
}
