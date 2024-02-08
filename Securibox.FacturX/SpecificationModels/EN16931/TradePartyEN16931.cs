using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.EN16931
{
    public class TradePartyEN16931
    {
        [XmlElement]
        public string[] ID { get; set; }
        [XmlElement]
        public Minimum.ID[] GlobalID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public BasicWL.LegalOrganization SpecifiedLegalOrganization { get; set; }
        public TradeContact DefinedTradeContact {  get; set; }
        public BasicWL.TradeAddress PostalTradeAddress { get; set; }
        public BasicWL.UniversalCommunication URIUniversalCommunication { get; set; }
        [XmlElement]
        public Minimum.TaxRegistration[] SpecifiedTaxRegistration { get; set; }
    }
}
