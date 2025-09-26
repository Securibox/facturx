using Securibox.FacturX.Models.Extended.Payment;

namespace Securibox.FacturX.Models.Extended
{
    public class PaymentPenaltyDiscount
    {
        public DateTime? BaseDateTime { get; internal set; }

        public PeriodMeasure? BasePeriodMeasure { get; internal set; }

        public decimal? BaseAmount { get; internal set; }

        public decimal? ActualAmount { get; internal set; }

        public decimal? Percentage { get; internal set; }
    }
}