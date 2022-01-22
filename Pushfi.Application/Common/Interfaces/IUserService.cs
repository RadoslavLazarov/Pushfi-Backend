using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Entities.Customer;

namespace Pushfi.Application.Common.Interfaces
{
    public interface IUserService
    {
        Task<UserModel> GetCurrentUserAsync();
        public Guid GetCurrentUserId();
        Task UpdateUserAsync(ApplicationUser user);
        Task<CustomerModel> GetCurrentCustomerAsync();
        Task<CustomerEntity> GetCurrentCustomerEntityAsync();
        Task UpdateCustomerAsync(CustomerEntity customer);
        Task DeleteCurrentCustomerAsync();
    }
}
