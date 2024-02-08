using System.Xml.Serialization;

namespace Securibox.FacturX.Schematron.Types
{
    /*
     * inclusion = element include {
     *   attribute href { uriValue },
     *   foreign-empty
     * }
     */
    [Serializable]
    public class Inclusion
    {
        [XmlAttribute(AttributeName = "href")]
        public string Href { get; set; }
    }
}
