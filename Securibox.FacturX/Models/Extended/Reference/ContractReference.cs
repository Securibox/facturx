namespace Securibox.FacturX.Models.Extended
{
    public class ContractReference : Models.BasicWL.ContractReference
    {
        public string? Type { get; internal set; }

        public DateTime? IssueDate { get; internal set; }
    }
}
