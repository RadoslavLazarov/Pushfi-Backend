using Pushfi.Domain.Enums;

namespace Pushfi.Application.Common.Models
{
    public class LatestOfferResponseModel
    {
        public string LowOffer { get; set; }
        public string HighOffer { get; set; }
        public string LowTermLoan { get; set; }
        public string HighTermLoan { get; set; }
        public double? TierFrom { get; set; }
        public double? TierTo { get; set; }
        public double? BackEndFee { get; set; }
        public EmailTemplateType Type { get; set; }
    }
}
