using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Minimum
{
    /// <summary>
    /// Represents a date string with format attribute (udt:DateType element)
    /// </summary>
    public class DateString
    {
        /// <summary>
        /// The date value (e.g., "20240220")
        /// </summary>
        [XmlText]
        public string Value { get; set; }

        /// <summary>
        /// The date format code (e.g., "102" for YYYYMMDD)
        /// </summary>
        [XmlAttribute("format")]
        public string Format { get; set; }
    }
}
