using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    [XmlRoot(
        Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
    )]
    public class ExchangedDocument
    {
        public Minimum.ID ID { get; set; }
        public string Name { get; set; }
        public string TypeCode { get; set; }
        public Minimum.IssueDateTime IssueDateTime { get; set; }
        public BasicWL.IndicatorType CopyIndicator { get; set; }
        public SpecifiedPeriodExtended EffectiveSpecifiedPeriod { get; set; }

        [XmlElement]
        public string[] LanguageID { get; set; }

        [XmlElement]
        public NoteExtended[] IncludedNote { get; set; }
    }
}
