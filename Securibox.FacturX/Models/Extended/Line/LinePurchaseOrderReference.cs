namespace Securibox.FacturX.Models.Extended
{
    public class LinePurchaseOrderReference : EN16931.LinePurchaseOrderReference
    {
        public string? AssignedId { get; private set; }
        public DateTime? IssueDate { get; private set; }

        internal LinePurchaseOrderReference(string? lineId) : base(lineId) { }

        internal void AddAssignedId(string? assignedId) => AssignedId = assignedId;
        internal void AddIssueDate(DateTime? issueDate) => IssueDate = issueDate;
    }
}
