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

namespace Pushfi.Application.Customer.Handlers
{
    public class SendOfferHandler : IRequestHandler<SendOfferCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IEnfortraService _enfortraService;
        private readonly IEmailService _emailService;
        private readonly SendGridConfiguration _sendGridConfiguration;

        public SendOfferHandler(
            IApplicationDbContext context,
            IUserService userService,
            IEnfortraService enfortraService,
            IEmailService _emailService,
            IOptionsMonitor<SendGridConfiguration> optionsMonitor)
        {
            this._context = context;
            this._userService = userService;
            this._enfortraService = enfortraService;
            this._emailService = _emailService;
            this._sendGridConfiguration = optionsMonitor.CurrentValue;
        }

        public async Task<Unit> Handle(SendOfferCommand request, CancellationToken cancellationToken)
        {
            var customer = await this._userService.GetCurrentCustomerAsync();

            if (customer == null)
            {
                // TODO
                throw new Exception();
            }

            var creditReport = await _enfortraService.GetFullCreditReportAsync(customer.Email);

            if (bool.Parse(creditReport.Root.MergeCreditReport.SB168Frozen.TUC))
            {
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
                // send reject email
                // return;
            }
            var tierString = tier[0].ToString(CultureInfo.InvariantCulture) + "% - " + tier[1].ToString(CultureInfo.InvariantCulture) + "%";


            var totalFundingAchieved = 9.9; // TODO: get value from brokers form
            var totalFundingAchievedString = totalFundingAchieved.ToString(CultureInfo.InvariantCulture) + "% of Total Funding Achieved";

            var logoHeight = 75; // TODO: get from broker db table

            //var rootDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            //var path = rootDirectory + "/Pushfi.Domain/Resources/EmailTemplates/CreditOffer.html";
            //var htmlTemplate = File.ReadAllText(path);

            var emailTemplate = await _emailService.GetEmailTemplateAsync(EmailTemplateType.CreditOffer);

            var sb = new StringBuilder(emailTemplate.HtmlContent);
            sb.Replace("@@logoHeight@@", logoHeight.ToString())
              .Replace("@@names@@", customer.FirstName + " " + customer.LastName)
              .Replace("@@offers@@", offersString)
              .Replace("@@tier@@", tierString)
              .Replace("@@totalFundingAchieved@@", totalFundingAchievedString);

            var htmlContent = sb.ToString();

            var email = new EmailModel()
            {
                //Message = "test message",
                Receiver = customer.Email,
                Sender = emailTemplate.Sender,
                Subject = emailTemplate.Subject,
                HtmlContent = htmlContent
            };

            await _emailService.SendAsync(email);

            var emailHistoryEntity = new CustomerEmailHistoryEntity()
            {
                LowOffer = offers[0],
                HighOffer = offers[1],
                TierFrom = tier[0],
                TierTo = tier[1],
                TotalFundingAchieved = totalFundingAchieved
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
    }
}
