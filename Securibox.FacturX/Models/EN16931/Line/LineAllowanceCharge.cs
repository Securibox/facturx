namespace Securibox.FacturX.Models.EN16931
{
    public class LineAllowanceCharge : Models.Basic.LineAllowanceCharge
    {
        public decimal? Percentage { get; internal set; }

        public decimal? BaseAmount { get; internal set; }
    }
}