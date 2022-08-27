﻿using MediatR;
using Pushfi.Application.Common.Models.Authentication;

namespace Pushfi.Application.Customer.Commands
{
    public class GetBrokerCustomersCommand : IRequest<List<CustomerModel>>
    {
    }
}
