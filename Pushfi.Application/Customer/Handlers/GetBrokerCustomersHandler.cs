using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Customer.Commands;
using Pushfi.Domain.Enums;
using System.Security;

namespace Pushfi.Application.Customer.Handlers
{
    public class GetBrokerCustomersHandler : IRequestHandler<GetBrokerCustomersCommand, List<CustomerModel>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IEnfortraService _enfortraService;

        public GetBrokerCustomersHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IUserService userService,
            IEnfortraService enfortraService)
        {
            this._context = context;
            this._mapper = mapper;
            this._userService = userService;
            this._enfortraService = enfortraService;
        }

        public async Task<List<CustomerModel>> Handle(GetBrokerCustomersCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = this._userService.GetCurrentUserId();
            var broker = this._context.Broker
                .Where(x => x.UserId == currentUserId)
                .Include(x => x.Customers).ThenInclude(x => x.User)
                .FirstOrDefault();

            if (broker == null)
            {
                throw new SecurityException();
            }
           
            var result = new List<CustomerModel>();

            foreach (var customer in broker.Customers)
            {
                result.Add(_mapper.Map<CustomerModel>(customer));
            }

            return result;
        }
    }
}
