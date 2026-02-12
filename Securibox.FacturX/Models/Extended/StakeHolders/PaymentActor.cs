using Securibox.FacturX.Models.BasicWL;
using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.Extended
{
    public class PaymentActor : BasicWL.PaymentActor
    {
        public string? RoleCode { get; internal set; }

        public EN16931.Contact? Contact { get; internal set; }

        public Minimum.PostalAddress? PostalAddress { get; internal set; }

        public ElectronicAddress? ElectronicAddress { get; internal set; }

        public IEnumerable<TaxRegistration?>? VatRegistrationList { get; internal set; }
    }
}
