using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.BasicWL
{
    [XmlRoot(
        Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
    )]
    public class ExchangedDocument
    {
        public Minimum.ID ID { get; set; }
        public string TypeCode { get; set; }
        public Minimum.IssueDateTime IssueDateTime { get; set; }

        [XmlElement]
        public Note[] IncludedNote { get; set; }
    }
}
