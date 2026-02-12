namespace Securibox.FacturX.Models.BasicWL
{
    public class PreviousInvoiceReference : IReference
    {
        public string? AssignedId { get; internal set; }

        public DateTime? IssueDate { get; internal set; }
    }
}
