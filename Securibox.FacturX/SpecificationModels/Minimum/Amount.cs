using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Minimum
{
    public class Amount
    {
        [XmlText]
        public decimal Value { get; set; }
        [XmlAttribute("currencyID")]
        public string CurrencyID { get; set; }
    }
}
