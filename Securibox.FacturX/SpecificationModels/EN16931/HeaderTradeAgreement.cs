using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.EN16931
{
    public class HeaderTradeAgreement
    {
        public string BuyerReference { get; set; }
        public TradePartyEN16931 SellerTradeParty { get; set; }
        public TradePartyEN16931 BuyerTradeParty { get; set; }
        public TradePartyEN16931 SellerTaxRepresentativeTradeParty { get; set; }
        public ReferencedDocumentEN16931 SellerOrderReferencedDocument { get; set; }
        public ReferencedDocumentEN16931 BuyerOrderReferencedDocument { get; set; }
        public ReferencedDocumentEN16931 ContractReferencedDocument { get; set; }
        [XmlElement]
        public ReferencedDocumentEN16931[] AdditionalReferencedDocument { get; set; }
        public ProcuringProject SpecifiedProcuringProject { get; set; }
    }
}
