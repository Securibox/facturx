using Securibox.FacturX.Models.BasicWL;

namespace Securibox.FacturX.Models.Extended
{
    public class LineVatDetails : Models.Basic.LineVatDetails
    {
        public decimal? VatAmount { get; set; }

        public Reason? VatExemptionReason { get; set; }
    }
}
