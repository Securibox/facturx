namespace Securibox.FacturX.Models.BasicWL
{
    public class TaxDistribution
    {
        public decimal? CategoryAmount { get; set; }

        public decimal? CategoryBaseAmount { get; set; }

        public string? CategoryType { get; set; }

        public string? CategoryCode { get; set; }

        public Reason? ExemptionReason { get; set; }

        public decimal? CategoryRate { get; set; }

        public string? AddedTaxPointDateCode { get; set; }
    }
}
