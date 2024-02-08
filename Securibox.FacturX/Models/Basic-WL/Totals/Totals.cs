using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.BasicWL
{
    public class Totals : Minimum.Totals
    {
        public TotalAmountAndCurrency? TotalVatAmountInCurrency { get; set; }
        public decimal? NetAmountSum { get;  set; }
        public decimal? ChargesSum { get;  set; }
        public decimal? AllowancesSum { get;  set; }
        public decimal? PaidAmount { get;  set; }
    }

}