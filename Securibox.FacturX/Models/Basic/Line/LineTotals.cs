namespace Securibox.FacturX.Models.Basic
{
    public class LineTotals
    {
        public decimal? LineNetAmount { get; set; }

        public LineTotals() { }

        public LineTotals(decimal? lineNetAmount) {
            LineNetAmount = lineNetAmount;
        }
    }
}
