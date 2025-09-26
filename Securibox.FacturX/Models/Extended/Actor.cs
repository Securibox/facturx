using Securibox.FacturX.Models.BasicWL;

namespace Securibox.FacturX.Models.Extended
{
    public abstract class Actor : IActor
    {
        public string? Id { get; internal set; }

        public IEnumerable<GlobalIdentification?>? GlobalIdentificationList { get; internal set; }

        public string? Name { get; internal set; }

        public LegalRegistration? LegalRegistration { get; internal set; }

        public Contact? Contact { get; internal set; }

        public PostalAddress? PostalAddress { get; internal set; }

        public ElectronicAddress? ElectronicAddress { get; internal set; }
    }
}