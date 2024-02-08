using System.Xml.Serialization;

namespace Securibox.FacturX.Schematron.Types
{
    public class Phase
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "active")]
        public Active[] ActivePatterns { get; set; }
    }
}
