namespace Pushfi.Application.Common.Models.Authentication
{
    public class UserModel
    {
        public UserModel()
        {
            this.RefreshTokens = new List<RefreshTokenModel>();
        }

        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public string AvatarColor { get; set; }

        public virtual IList<RefreshTokenModel> RefreshTokens { get; set; }
    }
}
