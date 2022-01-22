using MediatR;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Customer.Commands;

namespace Pushfi.Application.Customer.Handlers
{
    public class DeleteHandler : IRequestHandler<DeleteCommand>
    {
        private readonly IUserService _userService;
        private readonly IEnfortraService _enfortraService;

        public DeleteHandler(
            IUserService userService,
            IEnfortraService enfortraService)
        {
            this._userService = userService;
            this._enfortraService = enfortraService;
        }

        public async Task<Unit> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            await this._userService.DeleteCurrentCustomerAsync();

            return new Unit();
        }
    }
}
