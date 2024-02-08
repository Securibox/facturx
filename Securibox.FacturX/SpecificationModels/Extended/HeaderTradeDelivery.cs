namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class HeaderTradeDelivery
    {
        public SupplyChainConsignment RelatedSupplyChainConsignment { get; set; }
        public TradeParty ShipToTradeParty { get; set; }
        public TradeParty UltimateShipToTradeParty { get; set; }
        public TradeParty ShipFromTradeParty { get; set; }
        public BasicWL.SupplyChainEvent ActualDeliverySupplyChainEvent { get; set; }
        public EN16931.ReferencedDocumentEN16931 DespatchAdviceReferencedDocument { get; set; }
        public EN16931.ReferencedDocumentEN16931 ReceivingAdviceReferencedDocument { get; set; }
        public EN16931.ReferencedDocumentEN16931 DeliveryNoteReferencedDocument { get; set; }
    }
}