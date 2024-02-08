using Securibox.FacturX.Models.BasicWL;
using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.Extended
{
    public class DeliverToAddress : BasicWL.DeliverToAddress
    {
        public Minimum.LegalRegistration? LegalRegistration { get; internal set; }
        public EN16931.Contact? Contact { get; internal set; }
        public ElectronicAddress? ElectronicAddress { get; internal set; }
        public IEnumerable<TaxRegistration?>? VatRegistrationList { get; internal set; }
    }
}
