namespace Securibox.FacturX.Models.BasicWL
{
    public class DirectDebit : Minimum.DirectDebit
    {
        public string? CreditorReference { get; private set; }
        public string? RemittanceInformation { get; private set; }
        public string? VatAccountingCurrencyCode { get; private set; }

        public DirectDebit(
            string invoiceCurrencyCode,
            string? creditorReference,
            string? remittanceInformation,
            string? vatAccountingCurrencyCode
        )
            : base(invoiceCurrencyCode)
        {
            CreditorReference = creditorReference;
            RemittanceInformation = remittanceInformation;
            VatAccountingCurrencyCode = vatAccountingCurrencyCode;
        }
    }
}
