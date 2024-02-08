namespace Securibox.FacturX.SpecificationModels.BasicWL
{
    public class HeaderTradeAgreement
    {
        public string BuyerReference { get; set; }
        public TradeParty SellerTradeParty { get; set; }
        public TradeParty BuyerTradeParty { get; set; }
        public TradeParty SellerTaxRepresentativeTradeParty { get; set; }
        public ReferencedDocument BuyerOrderReferencedDocument { get; set; }
        public ReferencedDocument ContractReferencedDocument { get; set; }
    }
}
