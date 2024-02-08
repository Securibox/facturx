using Securibox.FacturX.Core;

namespace Securibox.FacturX.Models.BasicWL.Enum
{
    public class PaymentMethodTypeCode : Enumeration
    {
        public static PaymentMethodTypeCode Undefined = new PaymentMethodTypeCode(-1, "Undefined");
        public static PaymentMethodTypeCode Species = new PaymentMethodTypeCode(10, "Species");
        public static PaymentMethodTypeCode Check = new PaymentMethodTypeCode(20, "Check");
        public static PaymentMethodTypeCode Transfer = new PaymentMethodTypeCode(30, "Transfer");
        public static PaymentMethodTypeCode BankAccountPayment = new PaymentMethodTypeCode(42, "Payment on bank account");
        public static PaymentMethodTypeCode CreditCardPayment = new PaymentMethodTypeCode(48, "Payment by credit card");
        public static PaymentMethodTypeCode DirectDebit = new PaymentMethodTypeCode(49, "Direct debit");
        public static PaymentMethodTypeCode StandingAgreement = new PaymentMethodTypeCode(57, "Standing Agreement");
        public static PaymentMethodTypeCode SEPATransfer = new PaymentMethodTypeCode(58, "SEPATransfer");
        public static PaymentMethodTypeCode SEPADirectDebit = new PaymentMethodTypeCode(59, "SEPADirectDebit");
        public static PaymentMethodTypeCode Report = new PaymentMethodTypeCode(97, "Report");


        private PaymentMethodTypeCode(int id, string name) : base(id, name) { }

    }
}
