namespace Securibox.FacturX.Models.EN16931
{
    public class CreditTransfer : BasicWL.CreditTransfer
    {
        public string? PaymentAccountName { get; internal set; }

        public CreditTransfer(string ibanId, string? proprietaryId, string? paymentAccountName)
            : base(ibanId, proprietaryId) => PaymentAccountName = paymentAccountName;
    }
}
