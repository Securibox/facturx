using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Minimum
{
    [XmlRoot(Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100")]
    public class ExchangedDocument
    {
        public ID ID { get; set; }
        public string TypeCode { get; set; }
        [XmlElement(Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100")]
        public IssueDateTime IssueDateTime { get; set; }
    }
}
