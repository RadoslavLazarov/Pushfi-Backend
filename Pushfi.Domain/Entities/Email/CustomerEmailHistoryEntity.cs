using Pushfi.Domain.Entities.Customer;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pushfi.Domain.Entities.Email
{
    public class CustomerEmailHistoryEntity : TrackableEntityBase
    {
        public int LowOffer { get; set; }
        public int HighOffer { get; set; }
        public double TierFrom { get; set; }
        public double TierTo { get; set; }
        public double TotalFundingAchieved { get; set; }
    }
}
