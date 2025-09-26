using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.BasicWL
{
    public class SellerTaxRepresentative : IActor
    {
        public string Name { get; internal set; }

        public PostalAddress PostalAddress { get; internal set; }

        public TaxRegistration VatRegistration { get; internal set; }
    }
}