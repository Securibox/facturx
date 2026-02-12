using Securibox.FacturX.Models.Extended.Payment;

namespace Securibox.FacturX.Models.Extended
{
    public class AdvancePayment
    {
        public decimal? PaidAmount { get; internal set; }

        public DateTime? ReceivedPaymentDate { get; internal set; }

        public IEnumerable<IncludedTax>? IncludedTaxList { get; internal set; }
    }
}
