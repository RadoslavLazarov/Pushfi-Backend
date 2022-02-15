namespace Pushfi.Domain.Configuration
{
    public class SendGridConfiguration
    {
        public string ApiKey { get; set; }
        public string ApiKeyDevelopment { get; set; }
        public string Sender { get; set; }
        public string AdminReceiver { get; set; }
    }
}
