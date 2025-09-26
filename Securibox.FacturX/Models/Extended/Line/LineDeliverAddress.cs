using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.Extended
{
    public class LineDeliverAddress : Actor
    {
        public TaxRegistration? VatRegistration { get; internal set; }
    }
}