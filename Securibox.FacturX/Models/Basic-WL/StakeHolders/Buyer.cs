using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.BasicWL
{
    public class Buyer : Minimum.Buyer
    {
        public string? Id { get; set; }

        public GlobalIdentification? GlobalIdentification { get; set; }

        public TaxRegistration? VatRegistration { get; set; }

        public PostalAddress PostalAddress { get; set; }

        public ElectronicAddress? ElectronicAddress { get; set; }
    }
}
