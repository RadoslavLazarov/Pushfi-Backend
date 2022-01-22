
namespace Pushfi.Application.Common.Models.Enfortra.Response.GetFullCreditReport.GetFullCreditReportJson
{
    public class MergeCreditReportModel
    {
        public TransUnionCreditModel TotalMonthlyPayments { get; set; }
        public TransUnionCreditModel SB168Frozen { get; set; }
    }
}
