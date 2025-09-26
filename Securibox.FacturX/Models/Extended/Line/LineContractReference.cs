namespace Securibox.FacturX.Models.Extended
{
    public class LineContractReference : IReference
    {
        public string? LineId { get; private set; }

        public string? AssignedId { get; private set; }

        public DateTime? IssueDate { get; private set; }


        internal void AddLineId(string? lineId) => LineId = lineId;
        internal void AddAssignedId(string? assignedId) => AssignedId = assignedId;
        internal void AddIssueDate(DateTime? issueDate) => IssueDate = issueDate;
    }
}