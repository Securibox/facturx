namespace Securibox.FacturX.Models.Extended
{
    public class LineTotals : Basic.LineTotals
    {
        public decimal? TotalChargeWithoutVat { get; internal set; }

        public decimal? TotalAllowanceWithoutVat { get; internal set; }

        public decimal? TotalVatAmount { get; internal set; }

        public decimal? TotalAmountWithVat { get; internal set; }

        public decimal? TotalAllowanceAndCharge { get; internal set; }
    }
}