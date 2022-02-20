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
using Pushfi.Domain.Resources;

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
            //creditReport.Root.TransriskScores.ScoreFactors.TUC[0].type;
            //creditReport.Root.TransriskScores.ScoreFactors.TUC[0].factor;

            // Credit Freeze
            if (bool.Parse(creditReport.Root.MergeCreditReport.SB168Frozen.TUC))
            {
                await this.CreditFreezeAsync(customer, broker);
                return new Unit();
            }

            var monthlyIncome = (decimal)customer.MonthlyGrossIncomeAmount;
            var totalMonthlyPayments = Convert.ToDecimal(creditReport.Root.MergeCreditReport.TotalMonthlyPayments.TUC);
            var creditScoreTUC = Convert.ToInt32(creditReport.Root.TransriskScores.ScoreValue.TUC);
            var creditScoreEXP = Convert.ToInt32(creditReport.Root.TransriskScores.ScoreValue.EXP);
            var creditScoreEQF = Convert.ToInt32(creditReport.Root.TransriskScores.ScoreValue.EQF);

            var termLoans = PushfiCalculator.CalculateTermLoans(monthlyIncome, totalMonthlyPayments);
            var lowTermLoan = String.Format(CultureInfo.InvariantCulture, "{0:N0}", termLoans[0]);
            var highTermLoan = String.Format(CultureInfo.InvariantCulture, "{0:N0}", termLoans[1]);
            var termLoansString = "$" + lowTermLoan + " to " + "$" + highTermLoan;


            var offers = PushfiCalculator.CalculateOffers(termLoans);
            var lowOffer = String.Format(CultureInfo.InvariantCulture, "{0:N0}", offers[0]);
            var highOffer = String.Format(CultureInfo.InvariantCulture, "{0:N0}", offers[1]);
            var offersString = "$" + lowOffer + " - " + "$" + highOffer;

            var tier = PushfiCalculator.CalculateTier(creditScoreTUC);

            // Tier DECLINE
            if (tier.Count == 0)
            {
                await this.CreditDeclineAsync(customer, broker, creditScoreTUC);
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

            var scoreFactorsEntity = new List<ScoreFactorEntity>();
            var scoreFactors = "";
            foreach (var score in creditReport.Root.TransriskScores.ScoreFactors.TUC)
            {
                var scoreFactorEntity = new ScoreFactorEntity();
                scoreFactorEntity.Type = score.type;
                scoreFactorEntity.Factor = score.factor;
                scoreFactorsEntity.Add(scoreFactorEntity);

                var scoreTypeColor = score.type == "Positive" ? "green" : "red";

                var scoreStringBuilder = new StringBuilder(Strings.CreditOfferScoreFactor);
                scoreStringBuilder.Replace("@@scoreTypeColor@@", scoreTypeColor)
                    .Replace("@@scoreTypeName@@", score.type)
                    .Replace("@@scoreFactor@@", score.factor);

                scoreFactors += scoreStringBuilder.ToString();
            }

            //var rootDirectory = Directory.GetParent(Environment.CurrentDirectory).FullName;
            //var path = rootDirectory + "/Pushfi.Domain/Resources/EmailTemplates/CreditOffer.html";
            //var htmlTemplate = File.ReadAllText(path);

            var emailTemplate = await _emailService.GetEmailTemplateAsync(EmailTemplateType.CreditApproved);

            var sb = new StringBuilder(emailTemplate.HtmlContent);
            sb.Replace("@@logoUrl@@", logoUrl)
              .Replace("@@names@@", customer.FirstName + " " + customer.LastName)
              .Replace("@@offers@@", offersString)
              .Replace("@@termLoans@@", termLoansString)
              .Replace("@@tier@@", tierString)
              .Replace("@@backEndFee@@", backEndFeeString)
              .Replace("@@creditScoreTRU@@", creditReport.Root.TransriskScores.ScoreValue.TUC)
              .Replace("@@creditScoreEXP@@", creditReport.Root.TransriskScores.ScoreValue.EXP)
              .Replace("@@creditScoreEQU@@", creditReport.Root.TransriskScores.ScoreValue.EQF)
              .Replace("@@scoreFactors@@", scoreFactors);

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
                LowTermLoan = termLoans[0],
                HighTermLoan = termLoans[1],
                TierFrom = tier[0],
                TierTo = tier[1],
                BackEndFee = backEndFee,
                TotalMonthlyPayments = totalMonthlyPayments,
                CreditScoreTUC = creditScoreTUC,
                CreditScoreEXP = creditScoreEXP,
                CreditScoreEQF = creditScoreEQF,
                ScoreFactors = scoreFactorsEntity,
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

        private async Task CreditDeclineAsync(CustomerModel customer, BrokerEntity broker, int creditScore)
        {
            var emailType = EmailTemplateType.CreditDecline;
            var emailTemplate = await _emailService.GetEmailTemplateAsync(emailType);
            var subject = broker.CompanyName + "- " + emailTemplate.Subject + " " + customer.FirstName + " " + customer.LastName;

            var sb = new StringBuilder(emailTemplate.HtmlContent);
            sb.Replace("@@creditScore@@", creditScore.ToString());

            var htmlContent = sb.ToString();

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
                CreditScoreTUC = creditScore,
                Type = emailType
            };

            this._context.CustomerEmailHistory.Add(emailHistoryEntity);
            this._context.SaveChanges();
        }

        private async Task CreditFreezeAsync(CustomerModel customer, BrokerEntity broker)
        {
            var emailType = EmailTemplateType.CreditFreeze;
            //var emailTemplate = await _emailService.GetEmailTemplateAsync(emailType);
            var subject = broker.CompanyName + "- Credit Freeze: " + customer.FirstName + " " + customer.LastName;

            var customerEmail = new EmailModel()
            {
                Receiver = customer.Email,
                Sender = _sendGridConfiguration.Sender,
                Subject = subject,
                Message = "Condition: Credit Freeze present on bureau. Please visit the link to remove: https://www.transunion.com/credit-freeze"
                //HtmlContent = emailTemplate.HtmlContent
            };

            await _emailService.SendAsync(customerEmail);

            var adminEmail = new EmailModel()
            {
                Receiver = _sendGridConfiguration.AdminReceiver,
                Sender = _sendGridConfiguration.Sender,
                Subject = subject,
                Message = "Condition: Credit Freeze present on bureau. Please visit the link to remove: https://www.transunion.com/credit-freeze"
                //HtmlContent = emailTemplate.HtmlContent
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
