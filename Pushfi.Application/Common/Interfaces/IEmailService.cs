using Pushfi.Application.Common.Models;
using Pushfi.Domain.Enums;

namespace Pushfi.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailModel email);
        Task<EmailModel> GetEmailTemplateAsync(EmailTemplateType type);
    }
}
