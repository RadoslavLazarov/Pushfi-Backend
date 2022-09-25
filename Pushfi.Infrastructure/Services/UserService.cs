using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Domain.Configuration;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Entities.Broker;
using Pushfi.Domain.Entities.Customer;
using Pushfi.Domain.Enums;
using Pushfi.Domain.Exceptions;
using Pushfi.Domain.Resources;
using System.Security;
using System.Security.Claims;

namespace Pushfi.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly IEnfortraService _enfortraService;

        public UserService(
            IWebHostEnvironment env,
            IHttpContextAccessor accessor,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IApplicationDbContext context,
            IJwtService jwtService,
            IOptionsMonitor<JwtConfiguration> optionsMonitor,
            IEnfortraService enfortraService)
        {
            this._env = env;
            this._accessor = accessor;
            this._mapper = mapper;
            this._userManager = userManager;
            this._context = context;
            this._jwtService = jwtService;
            this._jwtConfiguration = optionsMonitor.CurrentValue;
            this._enfortraService = enfortraService;
        }

        #region Authentication

        public UserModel GetById(string id)
        {
            var user = this._userManager.Users
                .Include(x => x.RefreshTokens)
                .Where(x => x.Id.ToString() == id)
                .FirstOrDefault();

            if (user == null)
            {
                throw new EntityNotFoundException("User not found");
            }

            return this._mapper.Map<UserModel>(user);
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            var usersModel = new List<UserModel>();

            var users = this._userManager.Users.Include(x => x.RefreshTokens).ToList();
            foreach (var user in users)
            {
                var userModel = this._mapper.Map<UserModel>(user);
                userModel.Role = (await _userManager.GetRolesAsync(user))[0];
                usersModel.Add(userModel);
            }

            return usersModel;
        }

        public ApplicationUser GetUserByRefreshToken(string token)
        {
            var user = this._userManager.Users
                .Include(x => x.RefreshTokens)
                .SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                throw new SecurityException("Invalid token");
            }

            return user;
        }

        #endregion

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

        public async Task DeleteCustomerByUserIdAsync(Guid userId)
        {
            var customer = await this._context.Customer
                .Where(c => c.UserId == userId)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            if (customer == null)
            {
                throw new Exception(Strings.UserDoesNotExsists);
            }

            if (!this._env.IsDevelopment())
            {
                await this._enfortraService.CancelEnrollmentAsync(customer.User.Email);
            }
            var emails = this._context.CustomerEmailHistory.Where(x => x.CreatedById == userId).ToList();
            this._context.CustomerEmailHistory.RemoveRange(emails);
            this._context.Customer.Remove(customer);
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

        public async Task<RoleType> GetUserRoleTypeAsync(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            RoleType role;

            switch (userRoles[0])
            {
                case "Admin":
                    role = RoleType.Admin;
                    break;
                case "Broker":
                    role = RoleType.Broker;
                    break;
                case "Customer":
                    role = RoleType.Customer;
                    break;
                default:
                    role = RoleType.Customer;
                    break;
            }

            return role;
        }

        public async Task<string> GetUserFullNameAsync(ApplicationUser user)
        {
            var fullName = "";
            var role = await this.GetUserRoleTypeAsync(user);

            switch (role)
            {
                case RoleType.Broker:
                    var broker = await this._context.Broker
                            .Where(x => x.UserId == user.Id)
                            .FirstOrDefaultAsync();
                    fullName = broker.FullName;
                    break;
                case RoleType.Customer:
                    var customer = await this._context.Customer
                            .Where(x => x.UserId == user.Id)
                            .FirstOrDefaultAsync();
                    fullName = customer.FullName;
                    break;
            }

            return fullName;
        }
    }
}
