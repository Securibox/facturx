namespace Securibox.FacturX.Models.BasicWL
{
    public class AllowanceCharge
    {
        public bool? ChargeIndicator { get; internal set; }

        public decimal? ActualAmount { get; internal set; }

        public decimal? Percentage { get; internal set; }

        public decimal? BaseAmount { get; internal set; }

        public Reason? Reason { get; internal set; }

        public string? VatType { get; internal set; }

        public string? VatCategory { get; internal set; }

        public decimal? VatRate { get; internal set; }
    }
}
