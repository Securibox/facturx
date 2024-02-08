using Securibox.FacturX.Core;

namespace Securibox.FacturX.Models.Enums
{
    public class FacturXConformanceLevelType : Enumeration
    {
        public static FacturXConformanceLevelType Minimum = new FacturXConformanceLevelType(1, "Minimum");
        public static FacturXConformanceLevelType Basic = new FacturXConformanceLevelType(2, "Basic");
        public static FacturXConformanceLevelType BasicWL = new FacturXConformanceLevelType(3, "Basic-WL");
        public static FacturXConformanceLevelType EN16931 = new FacturXConformanceLevelType(4, "EN16931");
        public static FacturXConformanceLevelType Extended = new FacturXConformanceLevelType(5, "Extended");

        private FacturXConformanceLevelType(int id, string name) : base(id, name) { }
    }
}
