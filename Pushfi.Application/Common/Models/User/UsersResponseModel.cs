using Pushfi.Application.Common.Models.Authentication;

namespace Pushfi.Application.Common.Models.User
{
    public class UsersResponseModel
    {
        public UsersResponseModel()
        {
            this.Users = new List<UserModel>();
        }

        public List<UserModel> Users { get; set;}
    }
}
