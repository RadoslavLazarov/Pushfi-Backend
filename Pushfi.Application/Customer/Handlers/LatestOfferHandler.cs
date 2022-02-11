using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models;
using Pushfi.Application.Customer.Commands;

namespace Pushfi.Application.Customer.Handlers
{
    public class LatestOfferHandler : IRequestHandler<LatestOfferCommand, LatestOfferResponseModel>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public LatestOfferHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IUserService userService)
        {
            this._context = context;
            this._mapper = mapper;
            this._userService = userService;
        }

        public async Task<LatestOfferResponseModel> Handle(LatestOfferCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = this._userService.GetCurrentUserId();

            var offer = await this._context.CustomerEmailHistory
                .Where(x => x.CreatedById == currentUserId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            var model = this._mapper.Map<LatestOfferResponseModel>(offer);
            return model;
        }
    }
}
