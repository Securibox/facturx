namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class LineTradeDelivery
    {
        public Basic.Quantity BilledQuantity { get; set; }
        public Basic.Quantity ChargeFreeQuantity { get; set; }
        public Basic.Quantity PackageQuantity { get; set; }
        public TradeParty ShipToTradeParty { get; set; }
        public TradeParty UltimateShipToTradeParty { get; set; }
        public BasicWL.SupplyChainEvent ActualDeliverySupplyChainEvent { get; set; }
        public EN16931.ReferencedDocumentEN16931 DespatchAdviceReferencedDocument { get; set; }
        public EN16931.ReferencedDocumentEN16931 ReceivingAdviceReferencedDocument { get; set; }
        public EN16931.ReferencedDocumentEN16931 DeliveryNoteReferencedDocument { get; set; }
    }
}