using Securibox.FacturX.Models.BasicWL;

namespace Securibox.FacturX.Models.Basic
{
    public class LineAllowanceCharge
    {
        public bool? ChargeIndicator { get; internal set; }

        public decimal? ActualAmount { get; internal set; }

        public Reason? Reason { get; internal set; }
    }
}
