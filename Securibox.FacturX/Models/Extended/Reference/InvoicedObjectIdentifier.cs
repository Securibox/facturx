namespace Securibox.FacturX.Models.Extended
{
    public class InvoicedObjectIdentifier : EN16931.InvoicedObjectIdentifier
    {
        public DateTime? IssueDate { get; internal set; }
    }
}
