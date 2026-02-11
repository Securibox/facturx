namespace Securibox.FacturX.SpecificationModels.EN16931
{
    public class HeaderTradeDelivery
    {
        public TradePartyEN16931 ShipToTradeParty { get; set; }
        public BasicWL.SupplyChainEvent ActualDeliverySupplyChainEvent { get; set; }
        public ReferencedDocumentEN16931 DespatchAdviceReferencedDocument { get; set; }
        public ReferencedDocumentEN16931 ReceivingAdviceReferencedDocument { get; set; }
    }
}
