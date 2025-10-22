using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.Extended
{
    public class BuyerTaxRepresentative : Actor
    {
        public TaxRegistration? VatRegistration { get; internal set; }
    }
}