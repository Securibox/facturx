namespace Securibox.FacturX.Models.BasicWL
{
    public class PaymentInstructions
    {
        public string PaymentMethodCode { get; private set; }

        public string? DebitedAccountIBAN { get; private set; }

        public IEnumerable<CreditTransfer>? CreditTransferList { get; private set; }

        public PaymentInstructions(string paymentMethodCode, string? debitedAccountIBAN)
        {
            PaymentMethodCode = paymentMethodCode;
            DebitedAccountIBAN = debitedAccountIBAN;
        }

        public void AddBasicWLCreditTransferList(IEnumerable<CreditTransfer>? creditTransferList) => CreditTransferList = creditTransferList;

    }
}