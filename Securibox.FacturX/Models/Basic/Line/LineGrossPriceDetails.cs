namespace Securibox.FacturX.Models.Basic
{
    public class LineGrossPriceDetails
    {
        public decimal? GrossPrice { get; internal set; }

        public QuantityUnit? BaseQuantity { get; internal set; }

        public PriceAllowanceCharge? PriceDiscount { get; internal set; }
    }
}
