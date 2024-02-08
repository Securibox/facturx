using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.Extended
{
    public class ProductEndUser : Actor
    {
        public IEnumerable<TaxRegistration?>? VatRegistrationList { get; internal set; }
    }
}