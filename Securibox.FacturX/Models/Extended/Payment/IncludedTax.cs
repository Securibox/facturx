using Securibox.FacturX.Models.BasicWL;
using Securibox.FacturX.Models.BasicWL.Enum;

namespace Securibox.FacturX.Models.Extended.Payment
{
    public class IncludedTax
    {
        public decimal? VatAmount { get; internal set; }
        public string? VatType { get; internal set; }
        public Reason? VatExemptionReason { get; internal set; }
        public string? VatCategoryCode { get; internal set; }
        public decimal? VatRate { get; internal set; }
    }
}
