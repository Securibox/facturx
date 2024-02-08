using System.Xml.Serialization;

namespace Securibox.FacturX.Schematron.Types
{
    /*
     * ns = element ns {
     *   attribute uri { uriValue },
     *   attribute prefix { nameValue },
     *   foreign-empty
     * }
     */
    [Serializable]
    public class Ns
    {
        [XmlAttribute(AttributeName = "uri")]
        public string Uri { get; set; }

        [XmlAttribute(AttributeName = "prefix")]
        public string Prefix { get; set; }
    }
}
