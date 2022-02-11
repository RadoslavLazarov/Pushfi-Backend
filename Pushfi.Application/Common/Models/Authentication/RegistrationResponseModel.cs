namespace Pushfi.Application.Common.Models.Authentication
{
    public class RegistrationResponseModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public string Role { get; set; }
        public string CreditReportUrl { get; set; }
    }
}
