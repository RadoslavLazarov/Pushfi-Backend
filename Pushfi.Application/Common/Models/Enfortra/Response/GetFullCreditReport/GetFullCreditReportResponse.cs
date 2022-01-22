using System.Xml.Serialization;

namespace Pushfi.Application.Common.Models.Enfortra.Response.GetFullCreditReport
{
    [XmlRoot("GetFullCreditReportResponse")]
    public class GetFullCreditReportResponse : EnfortraResponse
    {
        [XmlElement(nameof(GetFullCreditReportResult))]
        public string GetFullCreditReportResult { get; set; }
    }
}
