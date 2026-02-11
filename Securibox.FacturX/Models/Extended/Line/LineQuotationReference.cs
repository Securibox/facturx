namespace Securibox.FacturX.Models.Extended
{
    public class LineQuotationReference : QuotationReference
    {
        public string? LineId { get; private set; }

        internal void AddLineId(string? lineId) => LineId = lineId;

        internal void AddAssignedId(string? assignedId) => AssignedId = assignedId;

        internal void AddIssueDate(DateTime? issueDate) => IssueDate = issueDate;
    }
}
