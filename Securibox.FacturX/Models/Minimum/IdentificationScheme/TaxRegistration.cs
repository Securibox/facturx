using Securibox.FacturX.Models.Minimum.Enum;

namespace Securibox.FacturX.Models.Minimum
{
    public class TaxRegistration
    {
        public string? Id { get; set; }

        public TaxSchemeId? Scheme { get; set; }
    }
}