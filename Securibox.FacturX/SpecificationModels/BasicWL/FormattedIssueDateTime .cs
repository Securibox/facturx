using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.BasicWL
{
    public class FormattedIssueDateTime
    {
        [XmlElement(Namespace = "urn:un:unece:uncefact:data:standard:QualifiedDataType:100")]
        public Minimum.DateTimeString DateTimeString { get; set; }
    }
}
