namespace Securibox.FacturX.Models.Extended
{
    public class LogisticsServiceCharge
    {
        public string? Description { get; internal set; }

        public decimal? AppliedAmount { get; internal set; }

        public IEnumerable<AppliedTax>? AppliedTaxList { get; internal set; }

    }
}