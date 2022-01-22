using AutoMapper;
using MediatR;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Application.Customer.Commands;
using Pushfi.Domain.Enums;

namespace Pushfi.Application.Customer.Handlers
{
    public class ProcessStatusHandler : IRequestHandler<ProcessStatusCommand, ProcessStatusModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IEnfortraService _enfortraService;

        public ProcessStatusHandler(
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

        public async Task<ProcessStatusModel> Handle(ProcessStatusCommand request, CancellationToken cancellationToken)
        {
            var model = new ProcessStatusModel();
            var customer = await _userService.GetCurrentCustomerAsync();

            // TODO: needs refactoring
            if (customer.ProcessStatus == ProcessStatus.Registration)
            {
                model.ProcessStatus = ProcessStatus.Registration;

                var kbaStatus = await this._enfortraService.GetKBAStatusAsync(customer.Email);
                if (kbaStatus is true)
                {
                    var customerEntity = await this._userService.GetCurrentCustomerEntityAsync();

                    model.ProcessStatus = ProcessStatus.Authentication;
                    customerEntity.ProcessStatus = ProcessStatus.Authentication;

                    await this._userService.UpdateCustomerAsync(customerEntity);
                }

                model.CreditReportUrl = (await this._enfortraService.GetCreditReportDetailsAsync(customer.Email)).CreditReportUrl;
            }
            if (customer.ProcessStatus == ProcessStatus.Authentication)
            {
                model.ProcessStatus = ProcessStatus.Authentication;
                model.CreditReportUrl = (await this._enfortraService.GetCreditReportDetailsAsync(customer.Email)).CreditReportUrl;
            }

            if (customer.ProcessStatus == ProcessStatus.GetOffer)
            {
                model.ProcessStatus = ProcessStatus.GetOffer;
                model.CreditReportUrl = (await this._enfortraService.GetCreditReportDetailsAsync(customer.Email)).CreditReportUrl;
            }

            return model;
        }
    }
}
