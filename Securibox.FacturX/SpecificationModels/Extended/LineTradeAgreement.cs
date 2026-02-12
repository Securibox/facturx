using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class LineTradeAgreement
    {
        public EN16931.ReferencedDocumentEN16931 BuyerOrderReferencedDocument { get; set; }
        public EN16931.ReferencedDocumentEN16931 QuotationReferencedDocument { get; set; }
        public EN16931.ReferencedDocumentEN16931 ContractReferencedDocument { get; set; }

        [XmlElement]
        public ReferencedDocumentExtended[] AdditionalReferencedDocument { get; set; }
        public TradePrice GrossPriceProductTradePrice { get; set; }
        public TradePrice NetPriceProductTradePrice { get; set; }

        [XmlElement]
        public EN16931.ReferencedDocumentEN16931[] UltimateCustomerOrderReferencedDocument { get; set; }
    }
}
