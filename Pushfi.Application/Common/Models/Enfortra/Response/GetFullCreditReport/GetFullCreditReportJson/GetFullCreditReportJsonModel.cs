using Newtonsoft.Json;

namespace Pushfi.Application.Common.Models.Enfortra.Response.GetFullCreditReport.GetFullCreditReportJson
{
    public class GetFullCreditReportJsonModel
    {
        [JsonProperty("ROOT")]
        public RootModel Root { get; set; }
    }
}
