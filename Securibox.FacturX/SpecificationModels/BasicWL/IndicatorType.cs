using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.BasicWL
{
    public class IndicatorType
    {
        [XmlElement("Indicator", Namespace = "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100")]
        public bool Indicator { get; set; }
    }
}