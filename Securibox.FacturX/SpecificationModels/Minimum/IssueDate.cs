using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Minimum
{
    public class IssueDate
    {
        [XmlElement(Namespace = "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100")]
        public DateString DateString { get; set; }
    }
}
