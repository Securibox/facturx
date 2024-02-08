namespace Securibox.FacturX.SpecificationModels.BasicWL
{
    public class TradeTax
    {
        public Minimum.Amount CalculatedAmount { get; set; }
        public string TypeCode { get; set; }
        public string ExemptionReason { get; set; }
        public Minimum.Amount BasisAmount { get; set; }
        public string CategoryCode { get; set; }
        public string ExemptionReasonCode { get; set; }
        public string DueDateTypeCode { get; set; }
        public decimal RateApplicablePercent { get; set; }
    }
}