using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class HeaderTradeAgreement
    {
        public string BuyerReference { get; set; }
        public TradeParty SellerTradeParty { get; set; }
        public TradeParty BuyerTradeParty { get; set; }
        public TradeParty SalesAgentTradeParty { get; set; }
        public TradeParty BuyerTaxRepresentativeTradeParty { get; set; }    
        public TradeParty SellerTaxRepresentativeTradeParty { get; set; }
        public TradeParty ProductEndUserTradeParty { get; set; }
        public TradeDeliveryTerms ApplicableTradeDeliveryTerms { get; set; }
        public EN16931.ReferencedDocumentEN16931 SellerOrderReferencedDocument { get; set; }
        public EN16931.ReferencedDocumentEN16931 BuyerOrderReferencedDocument { get; set; }
        public EN16931.ReferencedDocumentEN16931 QuotationReferencedDocument { get; set; }
        public EN16931.ReferencedDocumentEN16931 ContractReferencedDocument { get; set; }
        [XmlElement]
        public EN16931.ReferencedDocumentEN16931[] AdditionalReferencedDocument { get; set; }
        public TradeParty BuyerAgentTradeParty { get; set; }
        public EN16931.ProcuringProject SpecifiedProcuringProject {  get; set; }
        [XmlElement]
        public EN16931.ReferencedDocumentEN16931[] UltimateCustomerOrderReferencedDocument {  get; set; }

    }
}