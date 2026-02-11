using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class LogisticsServiceCharge
    {
        public string Description { get; set; }
        public Minimum.Amount AppliedAmount { get; set; }

        [XmlElement]
        public TradeTaxExtended[] AppliedTradeTax { get; set; }
    }
}
