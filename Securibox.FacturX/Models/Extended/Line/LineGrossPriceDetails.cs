namespace Securibox.FacturX.Models.Extended
{
    public class LineGrossPriceDetails : Basic.LineGrossPriceDetails
    {
        public new PriceAllowanceCharge? PriceDiscount { get; internal set; }
        public Extended.PriceAllowanceCharge? PriceCharge { get; internal set; }
    }
}
