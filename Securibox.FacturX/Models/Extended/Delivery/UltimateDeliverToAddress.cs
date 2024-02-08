using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.Extended
{
    public class UltimateDeliverToAddress : Actor
    {
        public IEnumerable<TaxRegistration?>? VatRegistrationList { get; internal set; }
    }
}
