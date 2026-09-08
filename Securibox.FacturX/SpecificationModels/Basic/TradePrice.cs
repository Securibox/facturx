namespace Securibox.FacturX.SpecificationModels.Basic
{
    public class TradePrice
    {
        public Minimum.Amount ChargeAmount { get; set; }
        public Quantity BasisQuantity { get; set; }
        public TradeAllowanceCharge AppliedTradeAllowanceCharge { get; set; }
    }
}
