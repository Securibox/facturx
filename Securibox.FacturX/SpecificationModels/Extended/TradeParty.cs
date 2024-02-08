using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class TradeParty
    {
        public string ID { get; set; }
        [XmlElement]
        public Minimum.ID[] GlobalID { get; set; }
        public string Name { get; set; }
        public string RoleCode { get; set; }
        public string Description { get; set; }
        public LegalOrganization SpecifiedLegalOrganization { get; set; }
        public TradeContact DefinedTradeContact { get; set; }
        public TradeAddressExtended PostalTradeAddress { get; set; }
        public BasicWL.UniversalCommunication URIUniversalCommunication { get; set; }
        [XmlElement]
        public Minimum.TaxRegistration[] SpecifiedTaxRegistration { get; set; }
    }
}