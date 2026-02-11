using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.EN16931
{
    [XmlRoot(
        Namespace = "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100",
        ElementName = "CrossIndustryInvoice"
    )]
    public class CrossIndustryInvoice : SpecificationModels.ICrossIndustryInvoice
    {
        public Minimum.ExchangedDocumentContext ExchangedDocumentContext { get; set; }

        public BasicWL.ExchangedDocument ExchangedDocument { get; set; }

        public SupplyChainTradeTransaction SupplyChainTradeTransaction { get; set; }
    }
}
