namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class TradeSettlementLineMonetarySummation
    {
        public Minimum.Amount LineTotalAmount { get; set; }
        public Minimum.Amount ChargeTotalAmount { get; set; }
        public Minimum.Amount AllowanceTotalAmount { get; set; }
        public Minimum.Amount TaxTotalAmount { get; set; }
        public Minimum.Amount GrandTotalAmount { get; set; }
        public Minimum.Amount TotalAllowanceChargeAmount { get; set; }
    }
}
