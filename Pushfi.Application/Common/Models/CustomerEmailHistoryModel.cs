using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pushfi.Application.Common.Models
{
    public class CustomerEmailHistoryModel : TrackableModelBase
    {
        public int LowOffer { get; set; }
        public int HighOffer { get; set; }
        public int LowTermLoan { get; set; }
        public int HighTermLoan { get; set; }
        public double TierFrom { get; set; }
        public double TierTo { get; set; }
        public double BackEndFee { get; set; }
    }
}
