using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Basic
{
    public class Quantity
    {
        [XmlText]
        public decimal Value { get; set; }

        [XmlAttribute("unitCode")]
        public string UnitCode { get; set; }
    }
}