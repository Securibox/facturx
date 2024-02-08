using Securibox.FacturX.Models.Enums;

namespace Securibox.FacturX.Models
{
    public class FacturXMetadata
    {
        public string DocumentType { get; set; }

        public string DocumentFileName { get; set; }

        public string Version { get; set; }

        public FacturXConformanceLevelType ConformanceLevel { get; set; }
    }
}
