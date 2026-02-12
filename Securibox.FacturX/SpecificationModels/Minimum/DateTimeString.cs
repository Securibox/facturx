using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Minimum
{
    public class DateTimeString
    {
        [XmlText]
        public string Value { get; set; }

        [XmlAttribute("format")]
        public string Format { get; set; }
    }
}
