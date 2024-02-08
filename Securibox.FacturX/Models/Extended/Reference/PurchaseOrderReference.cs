namespace Securibox.FacturX.Models.Extended
{
    public class PurchaseOrderReference : Minimum.PurchaseOrderReference
    {
        public DateTime? IssueDate { get; internal set; }
    }
}