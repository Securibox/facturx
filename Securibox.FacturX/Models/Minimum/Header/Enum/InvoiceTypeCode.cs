using Securibox.FacturX.Core;

namespace Securibox.FacturX.Models.Minimum.Enum
{
    public class InvoiceTypeCode : Enumeration
    {
        public static InvoiceTypeCode Undefined = new InvoiceTypeCode(-1, "Undefined");
        public static InvoiceTypeCode CommercialInvoice = new InvoiceTypeCode(
            380,
            "Commercial Invoice"
        );
        public static InvoiceTypeCode CreditNote = new InvoiceTypeCode(381, "Credit Note");
        public static InvoiceTypeCode CorrectedInvoice = new InvoiceTypeCode(
            384,
            "Corrected Invoice"
        );
        public static InvoiceTypeCode SelfBilledInvoice = new InvoiceTypeCode(
            389,
            "Self Billed Invoice"
        );
        public static InvoiceTypeCode SelfBilledCreditNote = new InvoiceTypeCode(
            261,
            "Self Billed Credit Note"
        );
        public static InvoiceTypeCode PrePaymentInvoice = new InvoiceTypeCode(
            386,
            "Prepayment Invoice"
        );
        public static InvoiceTypeCode InvoiceInformation = new InvoiceTypeCode(
            751,
            "Invoice Information"
        );

        private InvoiceTypeCode(int id, string name)
            : base(id, name) { }
    }
}
