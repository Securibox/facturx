using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.Extended
{
    public class Totals : EN16931.Totals
    {
        public new TotalAmountAndCurrency? TotalAmountWithoutVat { get; internal set; }

        public new TotalAmountAndCurrency? TotalAmountWithVat { get; internal set; }
    }
}