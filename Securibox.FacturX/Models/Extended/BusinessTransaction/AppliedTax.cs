using Securibox.FacturX.Models.BasicWL.Enum;

namespace Securibox.FacturX.Models.Extended
{
    public class AppliedTax
    {
        public string? VatType { get; internal set; }

        public string? VatCategoryCode { get; internal set; }

        public decimal? VatRate { get; internal set; }
    }
}