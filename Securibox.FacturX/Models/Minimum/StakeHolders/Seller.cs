namespace Securibox.FacturX.Models.Minimum
{
    public class Seller : IActor
    {
        public string? Name { get; set; }

        public LegalRegistration? LegalRegistration { get; set; }

        public PostalAddress? PostalAddress { get; set; }

        public TaxRegistration? VatRegistration { get; set; }
    }
}
