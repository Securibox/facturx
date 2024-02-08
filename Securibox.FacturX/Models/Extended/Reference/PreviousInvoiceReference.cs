namespace Securibox.FacturX.Models.Extended
{
    public class PreviousInvoiceReference : BasicWL.PreviousInvoiceReference
    {
        public string? Type { get; internal set; }
    }
}