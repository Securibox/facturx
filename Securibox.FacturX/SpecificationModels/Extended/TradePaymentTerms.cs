namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class TradePaymentTerms
    {
        public string Description { get; set; }
        public Minimum.IssueDateTime DueDateDateTime { get; set; }
        public Minimum.ID DirectDebitMandateID { get; set; }
        public Minimum.Amount PartialPaymentAmount { get; set; }
        public TradePaymentPenaltyTerms ApplicableTradePaymentPenaltyTerms { get; set; }
        public TradePaymentDiscountTerms ApplicableTradePaymentDiscountTerms { get; set; }
        public TradeParty PayeeTradeParty { get; set; }
    }
}
