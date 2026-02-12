using System.Xml.Serialization;

namespace Securibox.FacturX.Schematron.Types
{
    /* extends = element extends {
     * (attribute rule { xsd:IDREF }
     * | attribute href { uriValue }),
     * foreign-empty
     * }
     */
    public class Extends
    {
        [XmlAttribute(AttributeName = "rule")]
        public string Rule { get; set; }

        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
    }
}
