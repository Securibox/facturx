using Securibox.FacturX.Models.BasicWL;

namespace Securibox.FacturX.Models.Extended
{
    public class PriceAllowanceCharge : Basic.PriceAllowanceCharge
    {
        public decimal? Percentage { get; internal set; }

        public decimal? BaseAmount { get; internal set; }

        public Reason Reason { get; internal set; }
    }
}