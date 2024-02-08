namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class TradeCurrencyExchange
    {
        public string SourceCurrencyCode { get; set; }  
        public string TargetCurrencyCode { get; set; }
        public decimal ConversionRate { get; set; }
        public Minimum.IssueDateTime ConversionRateDateTime { get; set; }

    }
}