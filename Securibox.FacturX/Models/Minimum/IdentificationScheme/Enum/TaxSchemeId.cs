using Securibox.FacturX.Core;

namespace Securibox.FacturX.Models.Minimum.Enum
{
    public class TaxSchemeId : Enumeration
    {
        public static TaxSchemeId Undefined = new TaxSchemeId(-1, "Undefined");
        public static TaxSchemeId VA = new TaxSchemeId(1, "VA");
        public static TaxSchemeId FC = new TaxSchemeId(2, "FC");

        private TaxSchemeId(int id, string name) : base(id, name) { }
    }
}
