namespace Securibox.FacturX.Models.Minimum
{
    public class Totals
    {
        public TotalAmount? TotalAmountWithoutVat { get; set; }

        public TotalAmount? TotalAmountWithVat { get; set; }

        public TotalAmountAndCurrency? TotalVatAmount { get; set; }

        public decimal? AmountToBePaid { get; set; }
    }
}
