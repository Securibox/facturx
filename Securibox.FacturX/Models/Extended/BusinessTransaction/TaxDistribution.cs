namespace Securibox.FacturX.Models.Extended
{
    public class TaxDistribution : Models.EN16931.TaxDistribution
    {
        public decimal? LineTotalBaseAmount { get; internal set; }

        public decimal? AllowanceChargeBaseAmount { get; internal set; }
    }
}
