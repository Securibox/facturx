namespace Securibox.FacturX.Models.Extended
{
    public class TaxCurrencyExchange
    {
        public string? SourceCurrencyCode { get; internal set; }
        public string? TargetCurrencyCode { get; internal set; }
        public decimal? ConversionRate { get; internal set; }
        public DateTime? ConversionRateDate { get; internal set; }
    }
}
