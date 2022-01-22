namespace Pushfi.Application.Common.Models.Authentication
{
    public class LoginResponseModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Role { get; set; }
        public string CreditReportUrl { get; set; }
    }
}
