using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.EN16931
{
    public class Seller : BasicWL.Seller
    {
        public Contact? Contact { get; internal set; }
        public TaxRegistration? FiscalRegistration { get; internal set; }
        public string? AdditionalLegalInformation { get; internal set; }
    }
}