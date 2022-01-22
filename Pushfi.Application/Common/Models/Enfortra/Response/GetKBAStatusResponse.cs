using System.Xml.Serialization;

namespace Pushfi.Application.Common.Models.Enfortra.Response
{
    [XmlRoot("GetKBAStatusResponse")]
    public class GetKBAStatusResponse : EnfortraResponse
    {
        [XmlElement(nameof(GetKBAStatusResult))]
        public bool GetKBAStatusResult { get; set; }
    }
}
