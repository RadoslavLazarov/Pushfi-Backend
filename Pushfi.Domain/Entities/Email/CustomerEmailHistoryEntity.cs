using Pushfi.Domain.Entities.Customer;
using Pushfi.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pushfi.Domain.Entities.Email
{
    public class CustomerEmailHistoryEntity : TrackableEntityBase
    {
        public int? LowOffer { get; set; }
        public int? HighOffer { get; set; }
        public int? LowTermLoan { get; set; }
        public int? HighTermLoan { get; set; }
        public double? TierFrom { get; set; }
        public double? TierTo { get; set; }
        public double? BackEndFee { get; set; }

        #region Enfortra GetFullCreditReport
        public decimal? TotalMonthlyPayments { get; set; }
        public int? CreditScore { get; set; }
        #endregion

        public EmailTemplateType Type { get; set; }
    }
}
