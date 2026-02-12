namespace Securibox.FacturX.Models.EN16931
{
    public class PaymentInstructions : BasicWL.PaymentInstructions
    {
        public new IEnumerable<CreditTransfer>? CreditTransferList { get; private set; }

        public string? PaymentInformation { get; private set; }

        public CardInformation? PaymentCardInformation { get; private set; }

        public string? PaymentServiceProvider { get; private set; }

        public PaymentInstructions(string paymentMethodCode, string? debitedAccountIBAN)
            : base(paymentMethodCode, debitedAccountIBAN) { }

        public void AddEN16931CreditTransferList(IEnumerable<CreditTransfer>? creditTransferList) =>
            CreditTransferList = creditTransferList;

        public void AddPaymentInformation(string? paymentInformation) =>
            PaymentInformation = paymentInformation;

        public void AddPaymentCardInformation(CardInformation? paymentCardInformation) =>
            PaymentCardInformation = paymentCardInformation;

        public void AddPaymentServiceProvider(string? paymentServiceProvider) =>
            PaymentServiceProvider = paymentServiceProvider;
    }
}
