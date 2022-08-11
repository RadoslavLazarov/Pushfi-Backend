namespace Pushfi.Application.Common.Models.Authentication
{
    public class GetRefreshTokensResponseModel
    {
        public GetRefreshTokensResponseModel()
        {
            this.RefreshTokens = new List<RefreshTokenModel>();
        }

        public List<RefreshTokenModel> RefreshTokens { get; set; }
    }
}
