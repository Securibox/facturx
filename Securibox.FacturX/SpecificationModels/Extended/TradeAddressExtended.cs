using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class TradeAddressExtended 
    {
        public string PostcodeCode { get; set; }
        public string LineOne { get; set; }
        public string LineTwo { get; set; }
        public string LineThree { get; set; }
        public string CityName { get; set; }
        public string CountryID { get; set; }
        [XmlElement]
        public string[] CountrySubDivisionName { get; set; }
    }
}
