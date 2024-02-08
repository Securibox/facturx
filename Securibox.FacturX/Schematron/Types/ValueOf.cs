using System.Xml.Serialization;

namespace Securibox.FacturX.Schematron.Types
{
    /* value-of = element value-of {
     *   attribute select { pathValue },
     *   foreign-empty
     * }
     */
    [Serializable]
    public class ValueOf
    {
        [XmlAttribute(AttributeName = "select")]
        public string Select { get; set; }
    }
}
