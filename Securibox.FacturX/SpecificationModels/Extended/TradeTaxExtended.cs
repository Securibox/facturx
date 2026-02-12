namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class TradeTaxExtended
    {
        public Minimum.Amount CalculatedAmount { get; set; }
        public string TypeCode { get; set; }
        public string ExemptionReason { get; set; }
        public Minimum.Amount BasisAmount { get; set; }
        public Minimum.Amount LineTotalBasisAmount { get; set; }
        public Minimum.Amount AllowanceChargeBasisAmount { get; set; }
        public string CategoryCode { get; set; }
        public string ExemptionReasonCode { get; set; }
        public Minimum.IssueDateTime TaxPointDate { get; set; }
        public string DueDateTypeCode { get; set; }
        public decimal RateApplicablePercent { get; set; }
    }
}
