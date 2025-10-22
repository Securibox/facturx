using Securibox.FacturX.Models.Basic;

namespace Securibox.FacturX.Models.Extended
{
    public class LineDeliveryDetails : Basic.LineDeliveryDetails
    {
        public QuantityUnit? ChargeFreeQuantity { get; internal set; }

        public QuantityUnit? PackageQuantity { get; internal set; }

        public DateTime? DeliveryDate { get; internal set; }

        public LineDeliverAddress? DeliverToAddress { get; set; }

        public LineDeliverAddress? UltimateDeliverToAddress { get; set; }

        public LineDespacthAdviceReference? DespatchAdviceReference { get; internal set; }

        public LineReceivingAdviceReference? ReceivingAdviceReference { get; internal set; }

        public LineDeliveryNoteReference? DeliveryNoteReference { get; internal set; }
    }
}