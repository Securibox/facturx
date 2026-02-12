namespace Securibox.FacturX.Models.Extended
{
    public class DeliveryDetails : EN16931.DeliveryDetails
    {
        public new ReceivingAdviceReference? ReceivingAdviceReference { get; internal set; }

        public new DespacthAdviceReference? DespatchAdviceReference { get; internal set; }

        public DeliveryNoteReference? DeliveryNoteReference { get; internal set; }

        public new DeliverToAddress? DeliverToAddress { get; internal set; }

        public UltimateDeliverToAddress? UltimateDeliverToAddress { get; internal set; }

        public DeliverFromAddress? DeliverFromAddress { get; internal set; }

        public new DeliveryPeriod? InvoicingPeriod { get; internal set; }

        public string? DeliveryType { get; internal set; }

        public IEnumerable<ShippingTransportation>? ShippingTransportList { get; internal set; }
    }
}
