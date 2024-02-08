namespace Securibox.FacturX.SpecificationModels.Minimum
{
    public class HeaderTradeSettlement
    {
        public string InvoiceCurrencyCode { get; set; }
        public TradeSettlementHeaderMonetarySummation SpecifiedTradeSettlementHeaderMonetarySummation { get; set; }
    }
}
