using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Pushfi.Application.Common.Interfaces;
using Pushfi.Application.Common.Models;
using Pushfi.Application.Helpers;
using Pushfi.Application.Customer.Commands;
using Pushfi.Domain.Configuration;
using Pushfi.Domain.Entities.Authentication;
using Pushfi.Domain.Enums;
using System.Globalization;
using System.Text;
using Pushfi.Domain.Entities.Email;
using Pushfi.Application.Common.Constants;
using Pushfi.Application.Common.Models.Authentication;
using Pushfi.Domain.Entities.Broker;

namespace Pushfi.Application.Customer.Handlers
{
    public class SendOfferHandler : IRequestHandler<SendOfferCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IEnfortraService _enfortraService;
        private readonly IEmailService _emailService;
        private readonly SendGridConfiguration _sendGridConfiguration;
        private readonly AzureBlobStorageConfiguration _azureBlobStorageConfiguration;

        public SendOfferHandler(
            IApplicationDbContext context,
            IUserService userService,
            IEnfortraService enfortraService,
            IEmailService _emailService,
            IOptionsMonitor<SendGridConfiguration> sendGridConfiguration,
            IOptionsMonitor<AzureBlobStorageConfiguration> azureBlobStorageConfiguration)
        {
            this._context = context;
            this._userService = userService;
            this._enfortraService = enfortraService;
            this._emailService = _emailService;
            this._sendGridConfiguration = sendGridConfiguration.CurrentValue;
            this._azureBlobStorageConfiguration = azureBlobStorageConfiguration.CurrentValue;
        }

        public async Task<Unit> Handle(SendOfferCommand request, CancellationToken cancellationToken)
        {
            var customer = await this._userService.GetCurrentCustomerAsync();
            var broker = this._context.Broker.Where(x => x.Id == customer.BrokerId).FirstOrDefault();

            if (customer == null)
            {
                // TODO
                throw new Exception();
            }

            var emailType = EmailTemplateType.CreditApproved;

            var creditReport = await _enfortraService.GetFullCreditReportAsync(customer.Email);

            if (bool.Parse(creditReport.Root.MergeCreditReport.SB168Frozen.TUC))
            {
                emailType = EmailTemplateType.CreditFreeze;
                // send email with link https://www.transunion.com/credit-freeze 
                // return;
            }

            var monthlyIncome = (decimal)customer.MonthlyGrossIncomeAmount;
            var totalMonthlyPayments = Convert.ToDecimal(creditReport.Root.MergeCreditReport.TotalMonthlyPayments.TUC);
            var creditScore = Convert.ToInt32(creditReport.Root.TransriskScores.ScoreValue.TUC);

            var offers = PushfiCalculator.CalculateOffers(monthlyIncome, totalMonthlyPayments);
            var lowOffer = String.Format(CultureInfo.InvariantCulture, "{0:N0}", offers[0]);
            var highOffer = String.Format(CultureInfo.InvariantCulture, "{0:N0}", offers[1]);
            var offersString = "$" + lowOffer + " - " + "$" + highOffer;

            var tier = PushfiCalculator.CalculateTier(creditScore);

            // Tier DECLINE
            if (tier.Count == 0)
            {
                await this.CreditDecline(customer);

                return new Unit();
            }
            var tierString = tier[0].ToString(CultureInfo.InvariantCulture) + "% - " + tier[1].ToString(CultureInfo.InvariantCulture) + "%";

            var backEndFee = broker.BackEndFee;
            var backEndFeeString = backEndFee.ToString(CultureInfo.InvariantCulture) + "% of Total Funding Achieved";

            var logoUrl = !string.IsNullOrWhiteSpace(broker.LogoImage?.Url) ?
                broker.LogoImage.Url :
                this._azureBlobStorageConfiguration.BaseUrl +
                this._azureBlobStorageConfiguration.ContainerName +
                AzureBlobStorageConstants.BrokerDefaultLogoImagePath;

            var logoHeight = broker.LogoImage?.Height ?? AzureBlobStorageConstants.BrokerDefaultLogoImageHeight;

            //var rootDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            //var path = rootDirectory + "/Pushfi.Domain/Resources/EmailTemplates/CreditOffer.html";
            //var htmlTemplate = File.ReadAllText(path);

            var emailTemplate = await _emailService.GetEmailTemplateAsync(EmailTemplateType.CreditApproved);

            var sb = new StringBuilder(emailTemplate.HtmlContent);
            sb.Replace("@@logoUrl@@", logoUrl)
              .Replace("@@logoHeight@@", logoHeight.ToString())
              .Replace("@@names@@", customer.FirstName + " " + customer.LastName)
              .Replace("@@offers@@", offersString)
              .Replace("@@tier@@", tierString)
              .Replace("@@backEndFee@@", backEndFeeString);

            var htmlContent = sb.ToString();
            var subject = customer.FirstName + " " + customer.LastName + " " + emailTemplate.Subject;

            var customerEmail = new EmailModel()
            {
                Receiver = customer.Email,
                Sender = _sendGridConfiguration.Sender,
                Subject = subject,
                HtmlContent = htmlContent
            };
            await _emailService.SendAsync(customerEmail);

            var adminEmail = new EmailModel()
            {
                Receiver = _sendGridConfiguration.AdminReceiver,
                Sender = _sendGridConfiguration.Sender,
                Subject = subject,
                HtmlContent = htmlContent
            };
            await _emailService.SendAsync(adminEmail);

            var emailHistoryEntity = new CustomerEmailHistoryEntity()
            {
                LowOffer = offers[0],
                HighOffer = offers[1],
                TierFrom = tier[0],
                TierTo = tier[1],
                BackEndFee = backEndFee,
                TotalMonthlyPayments = totalMonthlyPayments,
                CreditScore = creditScore,
                Type = emailType
            };

            this._context.CustomerEmailHistory.Add(emailHistoryEntity);
            await this._context.SaveChangesAsync(cancellationToken);

            if (customer.ProcessStatus != ProcessStatus.GetOffer)
            {
                var customerEntity = await this._userService.GetCurrentCustomerEntityAsync();
                customerEntity.ProcessStatus = ProcessStatus.GetOffer;
                await this._userService.UpdateCustomerAsync(customerEntity);
            }

            return new Unit();
        }

        private async Task CreditDecline(CustomerModel customer)
        {
            var emailType = EmailTemplateType.CreditDecline;
            var emailTemplate = await _emailService.GetEmailTemplateAsync(emailType);

            var customerEmail = new EmailModel()
            {
                Receiver = customer.Email,
                Sender = _sendGridConfiguration.Sender,
                Subject = emailTemplate.Subject,
                HtmlContent = emailTemplate.HtmlContent
            };

            await _emailService.SendAsync(customerEmail);

            var adminEmail = new EmailModel()
            {
                Receiver = _sendGridConfiguration.AdminReceiver,
                Sender = _sendGridConfiguration.Sender,
                Subject = emailTemplate.Subject,
                HtmlContent = emailTemplate.HtmlContent
            };
            await _emailService.SendAsync(adminEmail);

            var emailHistoryEntity = new CustomerEmailHistoryEntity()
            {
                Type = emailType
            };

            this._context.CustomerEmailHistory.Add(emailHistoryEntity);
            this._context.SaveChanges();
        }
    }
}
