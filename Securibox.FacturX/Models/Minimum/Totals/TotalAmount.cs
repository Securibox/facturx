namespace Securibox.FacturX.Models.Minimum
{
    public class TotalAmount
    {
        public decimal? Amount { get; set; }

        public TotalAmount() { }

        public TotalAmount(decimal? amount)
        {
            Amount = amount;
        }   
    }
}