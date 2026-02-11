using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    [XmlRoot(
        Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
    )]
    public class ExchangedDocumentContext
    {
        public BasicWL.IndicatorType TestIndicator { get; set; }
        public Minimum.DocumentContextParameter BusinessProcessSpecifiedDocumentContextParameter { get; set; }
        public Minimum.DocumentContextParameter GuidelineSpecifiedDocumentContextParameter { get; set; }
    }
}
