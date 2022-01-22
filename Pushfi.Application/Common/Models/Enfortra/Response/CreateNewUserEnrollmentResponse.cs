using System.Xml.Serialization;

namespace Pushfi.Application.Common.Models.Enfortra.Response
{
	[XmlRoot("CreateNewUserEnrollmentResponse")]
	public class CreateNewUserEnrollmentResponse : EnfortraResponse
	{
		[XmlElement(nameof(CreateNewUserEnrollmentResult))]
		public long CreateNewUserEnrollmentResult { get; set; }
	}
}
