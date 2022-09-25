using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Entities.Broker;
using Pushfi.Domain.Entities.Customer;
using Pushfi.Domain.Enums;

namespace Pushfi.Application.Common.Interfaces
{
    public interface IUserService
    {
        #region Authentication

        UserModel GetById(string id);

        Task<List<UserModel>> GetAllAsync();

        ApplicationUser GetUserByRefreshToken(string token);

        #endregion

        Task<UserModel> GetCurrentUserAsync();

        Guid GetCurrentUserId();

        Task UpdateUserAsync(ApplicationUser user);

        Task<CustomerModel> GetCurrentCustomerAsync();

        Task<CustomerEntity> GetCurrentCustomerEntityAsync();

        Task UpdateCustomerAsync(CustomerEntity customer);

        Task DeleteCurrentCustomerAsync();

        Task DeleteCustomerByUserIdAsync(Guid userId);

        Task<BrokerEntity> GetCurrentBrokerEntityAsync();

        Task<RoleType> GetUserRoleTypeAsync(ApplicationUser user);

        Task<string> GetUserFullNameAsync(ApplicationUser user);
    }
}
