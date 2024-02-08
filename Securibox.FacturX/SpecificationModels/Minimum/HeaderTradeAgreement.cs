namespace Securibox.FacturX.SpecificationModels.Minimum
{
    public class HeaderTradeAgreement
    {
        public string BuyerReference { get; set; }
        public TradeParty SellerTradeParty { get; set; }
        public TradeParty BuyerTradeParty { get; set; }
        public ReferencedDocument BuyerOrderReferencedDocument { get; set; }
    }
}
