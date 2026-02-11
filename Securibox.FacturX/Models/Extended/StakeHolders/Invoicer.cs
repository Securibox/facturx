using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.Extended
{
    public class Invoicer : Actor
    {
        public TaxRegistration? VatRegistration { get; internal set; }
    }
}
