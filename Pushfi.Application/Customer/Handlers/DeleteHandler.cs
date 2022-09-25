using MediatR;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models;
using Pushfi.Application.Customer.Commands;

namespace Pushfi.Application.Customer.Handlers
{
    public class DeleteHandler : IRequestHandler<DeleteCommand, ResponseModel>
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

        public async Task<ResponseModel> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var model = new ResponseModel();

            try
            {
                await this._userService.DeleteCustomerByUserIdAsync(request.UserId);
                model.Success = true;
            }
            catch
            {
                model.Success = false;
            }

            return model;
        }
    }
}
