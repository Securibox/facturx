using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Basic
{
    [XmlRoot(
        Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
    )]
    public class SupplyChainTradeTransaction
    {
        [XmlElement]
        public SupplyChainTradeLineItem[] IncludedSupplyChainTradeLineItem { get; set; }
        public BasicWL.HeaderTradeAgreement ApplicableHeaderTradeAgreement { get; set; }
        public BasicWL.HeaderTradeDelivery ApplicableHeaderTradeDelivery { get; set; }
        public BasicWL.HeaderTradeSettlement ApplicableHeaderTradeSettlement { get; set; }
    }
}
