using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models;
using Pushfi.Domain.Configuration;
using Pushfi.Domain.Entities.Email;
using Pushfi.Domain.Enums;
using Pushfi.Infrastructure.Persistence;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Pushfi.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly SendGridConfiguration _sendGridConfiguration;

        public EmailService(
            IApplicationDbContext context,
            IMapper mapper,
            IOptionsMonitor<SendGridConfiguration> optionsMonitor)
        {
            this._context = context;
            this._mapper = mapper;
            this._sendGridConfiguration = optionsMonitor.CurrentValue;
        }

        public async Task SendAsync(EmailModel email)
        {
            var apiKey = this._sendGridConfiguration.ApiKeyDevelopment;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(email.Sender);
            var to = new EmailAddress(email.Receiver);

            try
            {
                var message = await Task.Run(() => MailHelper.CreateSingleEmail(from, to, email.Subject, email.Message, email.HtmlContent));
                var response = await client.SendEmailAsync(message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EmailModel> GetEmailTemplateAsync(EmailTemplateType type)
        {
            var template = await this._context.EmailTemplate.Where(x => x.Type == type).FirstOrDefaultAsync();
            var model = this._mapper.Map<EmailModel>(template);
            return model;
        }

        public async Task<CustomerEmailHistoryModel> GetLatestCustomerEmail(Guid UserId)
        {
            var email = await this._context.CustomerEmailHistory
                .Where(x => x.CreatedById == UserId)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            var emailModel = this._mapper.Map<CustomerEmailHistoryModel>(email);
            return emailModel;
        }
    }
}
