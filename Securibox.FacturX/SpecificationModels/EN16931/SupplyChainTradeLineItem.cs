using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.EN16931
{
    [XmlRoot(Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100")]
    public class SupplyChainTradeLineItem
    {
        public Basic.DocumentLineDocument AssociatedDocumentLineDocument { get; set; }
        public TradeProduct SpecifiedTradeProduct { get; set; }
        public LineTradeAgreement SpecifiedLineTradeAgreement { get; set; }
        public Basic.LineTradeDelivery SpecifiedLineTradeDelivery { get; set; }
        public LineTradeSettlement SpecifiedLineTradeSettlement { get; set; }
    }
}
