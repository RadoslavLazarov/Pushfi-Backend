using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Customer.Commands;
using Pushfi.Domain.Entities.Customer;
using Pushfi.Domain.Enums;
using Pushfi.Domain.Models;

namespace Pushfi.Application.Customer.Handlers
{
    public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersCommand, PageResult<CustomerModel>>
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

        public async Task<PageResult<CustomerModel>> Handle(GetAllCustomersCommand request, CancellationToken cancellationToken)
        {
            var totalCustomers = this._context.Customer.Count();
            var customersQuery = this._context.Customer
                .Include(x => x.User)
                .Include(x => x.Broker)
                .AsQueryable();

            if (request.Sorts.Any())
            {
                var sort = request.Sorts[0];

                switch (sort.Dir)
                {
                    case SortDirection.Asc:
                        if (sort.Field == "brokerCompanyName")
                        {
                            customersQuery = customersQuery.OrderBy(e => e.Broker.CompanyName);
                        }
                        else
                        {
                            customersQuery = customersQuery.OrderBy(e => EF.Property<object>(e, char.ToUpper(sort.Field[0]) + sort.Field.Substring(1)));
                        }

                        break;
                    case SortDirection.Desc:
                        if (sort.Field == "brokerCompanyName")
                        {
                            customersQuery = customersQuery.OrderByDescending(e => e.Broker.CompanyName);
                        }
                        else { 
                            customersQuery = customersQuery.OrderByDescending(e => EF.Property<object>(e, char.ToUpper(sort.Field[0]) + sort.Field.Substring(1)));
                        }
                        break;
                }            
            }

            var customers = customersQuery.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();

            var result = new List<CustomerModel>();

            foreach (var customer in customers)
            {
                result.Add(_mapper.Map<CustomerModel>(customer));
            }

            var pageResult = new PageResult<CustomerModel>();
            pageResult.TotalCount = totalCustomers;

            foreach (var customer in customers)
            {
                pageResult.Items.Add(_mapper.Map<CustomerModel>(customer));
            }

            return pageResult;
        }
    }
}
