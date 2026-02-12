using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.Extended
{
    public class SalesAgent : Actor
    {
        public TaxRegistration? VatRegistration { get; internal set; }
    }
}
