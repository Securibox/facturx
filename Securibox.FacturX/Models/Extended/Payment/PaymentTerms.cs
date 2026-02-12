namespace Securibox.FacturX.Models.Extended
{
    public class PaymentTerms : BasicWL.PaymentTerms
    {
        public decimal? PartialPaymentAmount { get; internal set; }

        public PaymentPenaltyDiscount? PaymentPenalty { get; internal set; }

        public PaymentPenaltyDiscount? PaymentDiscount { get; internal set; }
    }
}
