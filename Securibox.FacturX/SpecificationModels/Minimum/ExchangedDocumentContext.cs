using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Minimum
{
    [XmlRoot(
        Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
    )]
    public class ExchangedDocumentContext
    {
        public DocumentContextParameter BusinessProcessSpecifiedDocumentContextParameter { get; set; }
        public DocumentContextParameter GuidelineSpecifiedDocumentContextParameter { get; set; }
    }
}
