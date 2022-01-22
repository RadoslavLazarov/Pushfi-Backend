using System.Xml.Serialization;

namespace Pushfi.Application.Common.Models.Enfortra.Response
{
	[XmlRoot("GetCreditReportDetailsResponse")]
	public class GetCreditReportDetailsResponse : EnfortraResponse
	{
		[XmlElement(nameof(GetCreditReportDetailsResult))]
		public string GetCreditReportDetailsResult { get; set; }
	}
}
