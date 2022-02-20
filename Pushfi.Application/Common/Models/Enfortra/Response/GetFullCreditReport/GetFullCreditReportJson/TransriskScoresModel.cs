namespace Pushfi.Application.Common.Models.Enfortra.Response.GetFullCreditReport.GetFullCreditReportJson
{
    public class TransriskScoresModel
    {
        public TransUnionCreditModel ScoreValue { get; set; }
        public ScoreFactorsTransUnionCreditModel ScoreFactors { get; set; }
}
}
