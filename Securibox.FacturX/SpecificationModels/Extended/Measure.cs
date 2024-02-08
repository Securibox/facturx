using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class Measure
    {
        [XmlText]
        public decimal Value { get; set; }
        [XmlAttribute("unitCode")]
        public string UnitCode { get; set; }
    }
}