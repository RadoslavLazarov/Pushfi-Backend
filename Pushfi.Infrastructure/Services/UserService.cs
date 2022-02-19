using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Entities.Broker;
using Pushfi.Domain.Entities.Customer;
using Pushfi.Domain.Exceptions;
using Pushfi.Domain.Resources;
using System.Security.Claims;

namespace Pushfi.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IEnfortraService _enfortraService;

        public UserService(
            IHttpContextAccessor accessor,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IApplicationDbContext context,
            IEnfortraService enfortraService)
        {
            this._accessor = accessor;
            this._mapper = mapper;
            this._userManager = userManager;
            this._context = context;
            this._enfortraService = enfortraService;
        }

        public async Task<UserModel> GetCurrentUserAsync()
        {
            var userId = this.GetCurrentUserId();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var model = this._mapper.Map<UserModel>(user);

            return model;
        }

        public Guid GetCurrentUserId()
        {
            var userClaims = this._accessor?.HttpContext?.User;
            var userId = Guid.Parse(userClaims.FindFirstValue(ClaimTypes.NameIdentifier));
            return userId;
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task<CustomerModel> GetCurrentCustomerAsync()
        {
            var userId = this.GetCurrentUserId();

            var customer = await this._context.Customer
                .Where(c => c.UserId == userId)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            if (customer == null)
            {
                throw new EntityNotFoundException(string.Format(Strings.UserDoesNotExsists));
            }

            var customerModel = this._mapper.Map<CustomerModel>(customer);
            //customerModel.Email = customer.User.Email;
            return customerModel;
        }

        public async Task<CustomerEntity> GetCurrentCustomerEntityAsync()
        {
            var userId = this.GetCurrentUserId();

            var customer = await this._context.Customer
                .Where(c => c.UserId == userId)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            if (customer == null)
            {
                throw new EntityNotFoundException(string.Format(Strings.UserDoesNotExsists));
            }

            return customer;
        }

        //public async Task CreateCustomerAsync(CustomerEntity customer)
        //{
        //    await this._context.Customer.AddAsync(customer);
        //    await this._context.SaveChangesAsync(new CancellationToken());
        //}

        public async Task UpdateCustomerAsync(CustomerEntity customer)
        {     
            this._context.Customer.Update(customer);
            this._context.SaveChanges();
        }

        public async Task DeleteCurrentCustomerAsync()
        {
            var userId = this.GetCurrentUserId();
            var user = await this._userManager.FindByIdAsync(userId.ToString());

           if (user == null)
            {
                throw new Exception(Strings.UserDoesNotExsists);
            }

            await this._enfortraService.CancelEnrollmentAsync(user.Email);
            var userDeletion = await this._userManager.DeleteAsync(user);

            if (!userDeletion.Succeeded)
            {
                throw new Exception(Strings.UserDeletionFailed);
            }

            var emails = this._context.CustomerEmailHistory.Where(x => x.CreatedById == userId).ToList();
            this._context.CustomerEmailHistory.RemoveRange(emails);
            this._context.SaveChanges();
        }

        public async Task<BrokerEntity> GetCurrentBrokerEntityAsync()
        {
            var userId = this.GetCurrentUserId();

            var broker = await this._context.Broker
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();

            if (broker == null)
            {
                throw new EntityNotFoundException(string.Format(Strings.UserDoesNotExsists));
            }

            return broker;
        }
    }
}
