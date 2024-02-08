using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    [XmlRoot(Namespace = "urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100", ElementName = "CrossIndustryInvoice")]
    public class CrossIndustryInvoice : SpecificationModels.ICrossIndustryInvoice
    {
        public ExchangedDocumentContext ExchangedDocumentContext { get; set; }

        public ExchangedDocument ExchangedDocument { get; set; }

        public SupplyChainTradeTransaction SupplyChainTradeTransaction { get; set; }
    }
}
