using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Minimum
{
    public class IssueDateTime
    {
        [XmlElement(Namespace = "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100")]
        public DateTimeString DateTimeString { get; set; }
    }
}
