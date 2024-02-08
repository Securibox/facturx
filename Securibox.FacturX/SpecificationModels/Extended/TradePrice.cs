namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class TradePrice
    {
        public Minimum.Amount ChargeAmount { get; set; }
        public Basic.Quantity BasisQuantity { get; set; }
        public TradeAllowanceChargeExtended AppliedTradeAllowanceCharge { get; set; }
        public TradeTaxExtended IncludedTradeTax { get; set; }
    }
}