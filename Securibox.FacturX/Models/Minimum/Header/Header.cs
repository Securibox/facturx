namespace Securibox.FacturX.Models.Minimum
{
    public class Header
    {
        public string InvoiceNumber { get; private set; }

        public string InvoiceType { get; private set; }

        public DateTime EmissionDate { get; private set; }

        public Header(string invoiceNumber, string invoiceType, DateTime emissionDate)
        {
            InvoiceNumber = invoiceNumber;
            InvoiceType = invoiceType;
            EmissionDate = emissionDate;
        }
    }
}
