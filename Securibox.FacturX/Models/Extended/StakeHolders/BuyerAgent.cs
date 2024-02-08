using Securibox.FacturX.Models.BasicWL;
using Securibox.FacturX.Models.Minimum;

namespace Securibox.FacturX.Models.Extended
{
    public class BuyerAgent : Actor
    {
        public TaxRegistration? VatRegistration { get; internal set; }
    }
}