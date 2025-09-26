using Securibox.FacturX.Models.BasicWL;

namespace Securibox.FacturX.Models.Extended
{
    public class SellerTaxRepresentative : BasicWL.SellerTaxRepresentative
    {
        public string? Id { get; internal set; }

        public IEnumerable<GlobalIdentification>? GlobalIdentificationList { get; internal set; }

        public LegalRegistration? LegalRegistration { get; internal set; }

        public Contact? Contact { get; internal set; }

        public ElectronicAddress? ElectronicAddress { get; internal set; }
    }
}