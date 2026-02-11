namespace Securibox.FacturX.Models.BasicWL
{
    public class DeliveryDetails
    {
        public DeliverToAddress? DeliverToAddress { get; internal set; }

        public DateTime? DeliveryDate { get; internal set; }

        public DeliveryPeriod? InvoicingPeriod { get; internal set; }

        public DespacthAdviceReference? DespatchAdviceReference { get; internal set; }
    }
}
