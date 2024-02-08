namespace Securibox.FacturX.Models.Minimum
{
    public class DirectDebit
    {
        public string InvoiceCurrencyCode { get; private set; }

        public DirectDebit(string invoiceCurrencyCode) => InvoiceCurrencyCode = invoiceCurrencyCode;
    }
}
