using Securibox.FacturX.Models.Basic;

namespace Securibox.FacturX.Models.Extended
{
    public class AllowanceCharge : BasicWL.AllowanceCharge
    {
        public string? SequenceNumber { get; internal set; }
        public QuantityUnit? BaseQuantity { get; internal set; }
    }
}
