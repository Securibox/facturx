using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.BasicWL
{
    [XmlRoot(Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100")]
    public class SupplyChainTradeTransaction
    {
        public HeaderTradeAgreement ApplicableHeaderTradeAgreement { get; set; }
        public HeaderTradeDelivery ApplicableHeaderTradeDelivery { get; set; }
        public HeaderTradeSettlement ApplicableHeaderTradeSettlement { get; set; }
    }
}
