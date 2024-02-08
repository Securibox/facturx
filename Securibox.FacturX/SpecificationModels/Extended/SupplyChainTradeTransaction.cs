using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    [XmlRoot(Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100")]
    public class SupplyChainTradeTransaction
    {
        [XmlElement]
        public SupplyChainTradeLineItem[] IncludedSupplyChainTradeLineItem { get; set; }
        public HeaderTradeAgreement ApplicableHeaderTradeAgreement { get; set; }
        public TradeDelivery ApplicableHeaderTradeDelivery { get; set; }
        public HeaderTradeSettlement ApplicableHeaderTradeSettlement { get; set; }
    }
}