namespace Securibox.FacturX.SpecificationModels.Minimum
{
    public class TradeSettlementHeaderMonetarySummation
    {
        public Amount TaxBasisTotalAmount { get; set; }
        public Amount TaxTotalAmount { get; set; }
        public Amount GrandTotalAmount { get; set; }
        public Amount DuePayableAmount { get; set; }
    }
}
