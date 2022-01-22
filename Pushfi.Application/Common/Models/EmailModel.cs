using Pushfi.Domain.Enums;

namespace Pushfi.Application.Common.Models
{
    public class EmailModel
    {
        public EmailTemplateType Type { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string HtmlContent { get; set; }
    }
}
