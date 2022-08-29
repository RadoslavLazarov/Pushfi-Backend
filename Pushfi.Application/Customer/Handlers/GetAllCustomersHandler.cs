using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Customer.Commands;

namespace Pushfi.Application.Customer.Handlers
{
    public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersCommand, List<CustomerModel>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllCustomersHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<List<CustomerModel>> Handle(GetAllCustomersCommand request, CancellationToken cancellationToken)
        {
            var customers = this._context.Customer
                .Include(x => x.User)
                //.Include(x => x.Broker)
                .ToList();

            var result = new List<CustomerModel>();

            foreach (var customer in customers)
            {
                result.Add(_mapper.Map<CustomerModel>(customer));
            }

            return result;
        }
    }
}
