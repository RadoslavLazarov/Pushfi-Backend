using Pushfi.Domain.Enums;

namespace Pushfi.Application.Common.Models.Authentication
{
    public class ProcessStatusModel
    {
        public ProcessStatus ProcessStatus { get; set; }
        public string CreditReportUrl { get; set; }
    }
}
