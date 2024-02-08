using Securibox.FacturX.Models.Extended.Payment;

namespace Securibox.FacturX.Models.Extended
{
    public class LineNetPriceDetails : Basic.LineNetPriceDetails
    {
        public IncludedTax? IncludedTax { get; internal set; }
    }
}
