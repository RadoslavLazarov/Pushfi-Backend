using System.Xml.Serialization;

namespace Pushfi.Application.Common.Models.Enfortra.Response
{
    [XmlRoot("CancelEnrollmentResponse")]
    public class CancelEnrollmentResponse : EnfortraResponse
    {
        [XmlElement(nameof(CancelEnrollmentResult))]
        public bool CancelEnrollmentResult { get; set; }
    }
}
