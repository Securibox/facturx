namespace Securibox.FacturX.SpecificationModels.BasicWL
{
    public class HeaderTradeDelivery
    {
        public TradeParty ShipToTradeParty { get; set; }
        public SupplyChainEvent ActualDeliverySupplyChainEvent { get; set; }
        public ReferencedDocument DespatchAdviceReferencedDocument { get; set; }
    }
}