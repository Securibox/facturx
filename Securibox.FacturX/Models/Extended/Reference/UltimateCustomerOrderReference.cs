namespace Securibox.FacturX.Models.Extended
{
    public class UltimateCustomerOrderReference : IReference
    {
        public string? AssignedId { get; internal set; }
        public DateTime? IssueDate { get; internal set; }
    }
}