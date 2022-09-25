using MediatR;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Customer.Commands;

namespace Pushfi.Application.Customer.Handlers
{
    public class DeleteCurrentCustomerHandler : IRequestHandler<DeleteCurrentCustomerCommand>
    {
        private readonly IUserService _userService;
        private readonly IEnfortraService _enfortraService;

        public DeleteCurrentCustomerHandler(
            IUserService userService,
            IEnfortraService enfortraService)
        {
            this._userService = userService;
            this._enfortraService = enfortraService;
        }

        public async Task<Unit> Handle(DeleteCurrentCustomerCommand request, CancellationToken cancellationToken)
        {
            await this._userService.DeleteCurrentCustomerAsync();

            return new Unit();
        }
    }
}
