using Pushfi.Domain.Enums;

namespace Pushfi.Domain.Entities.Email
{
    public class EmailTemplateEntity : EntityBase
    {
        public EmailTemplateType Type { get; set; }
        public string Sender { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public string Message { get; set; }
    }
}
