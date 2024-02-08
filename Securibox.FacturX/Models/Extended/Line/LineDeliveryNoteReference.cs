namespace Securibox.FacturX.Models.Extended
{
    public class LineDeliveryNoteReference : Extended.DeliveryNoteReference
    {
        public string? LineId { get; internal set; }
    }
}