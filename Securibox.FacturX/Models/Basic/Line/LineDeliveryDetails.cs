namespace Securibox.FacturX.Models.Basic
{
    public class LineDeliveryDetails
    {
        public QuantityUnit InvoicedQuantity { get; internal set; }

        public BasicWL.DeliveryPeriod? DeliveryPeriod { get; internal set; }
    }
}
