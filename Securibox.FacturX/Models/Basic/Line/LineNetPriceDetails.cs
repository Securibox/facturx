namespace Securibox.FacturX.Models.Basic
{
    public class LineNetPriceDetails
    {
        public decimal? NetPrice { get; set; }

        public QuantityUnit? BaseQuantity { get; set; }

        public LineNetPriceDetails() { }

        public LineNetPriceDetails(decimal? netPrice, QuantityUnit? baseQuantity)
        {
            NetPrice = netPrice;
            BaseQuantity = baseQuantity;
        }

        public LineNetPriceDetails(decimal? netPrice, decimal? quantity)
        {
            NetPrice = netPrice;
            BaseQuantity = new QuantityUnit(quantity.Value, "C62");
        }
    }
}