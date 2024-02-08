using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.BasicWL
{
    public class TradeParty
    {
        [XmlElement]
        public Minimum.ID[] ID { get; set; }
        [XmlElement]
        public Minimum.ID[] GlobalID { get; set; }
        public string Name { get; set; }
        public LegalOrganization SpecifiedLegalOrganization { get; set; }
        public TradeAddress PostalTradeAddress { get; set; }
        public UniversalCommunication URIUniversalCommunication { get; set; }
        public Minimum.TaxRegistration SpecifiedTaxRegistration { get; set; }
    }
}
