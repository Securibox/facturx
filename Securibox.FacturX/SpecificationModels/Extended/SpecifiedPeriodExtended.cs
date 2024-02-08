namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class SpecifiedPeriodExtended
    {
        public string Description { get; set; }
        public Minimum.IssueDateTime StartDateTime { get; set; }
        public Minimum.IssueDateTime EndDateTime { get; set; }
        public Minimum.IssueDateTime CompleteDateTime { get; set; }
    }
}
