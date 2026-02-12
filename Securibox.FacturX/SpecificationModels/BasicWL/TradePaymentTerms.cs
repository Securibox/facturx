namespace Securibox.FacturX.SpecificationModels.BasicWL
{
    public class TradePaymentTerms
    {
        public string Description { get; set; }
        public Minimum.IssueDateTime DueDateDateTime { get; set; }
        public Minimum.ID DirectDebitMandateID { get; set; }
    }
}
