namespace Securibox.FacturX.Models.Extended
{
    public class DirectDebit : BasicWL.DirectDebit
    {
        public string? InvoiceIssuerReference { get; private set; }

        public DirectDebit(string invoiceCurrencyCode, string? creditorReference, string? remittanceInformation, string? vatAccountingCurrencyCode, string invoiceIssuerReference)
          : base(invoiceCurrencyCode, creditorReference, remittanceInformation, vatAccountingCurrencyCode) => InvoiceIssuerReference = invoiceIssuerReference;
    }
}
