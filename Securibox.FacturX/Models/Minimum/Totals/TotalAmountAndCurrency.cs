namespace Securibox.FacturX.Models.Minimum
{
    public class TotalAmountAndCurrency : TotalAmount
    {
        public string? Currency { get; set; }

        public TotalAmountAndCurrency() { }

        public TotalAmountAndCurrency(decimal? amount, string? currency)
            : base(amount)
        {
            Currency = currency;
        }
    }
}
