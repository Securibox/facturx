using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    [XmlRoot(
        Namespace = "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100"
    )]
    public class SupplyChainTradeLineItem
    {
        public DocumentLineDocument AssociatedDocumentLineDocument { get; set; }
        public TradeProduct SpecifiedTradeProduct { get; set; }
        public LineTradeAgreement SpecifiedLineTradeAgreement { get; set; }
        public LineTradeDelivery SpecifiedLineTradeDelivery { get; set; }
        public LineTradeSettlement SpecifiedLineTradeSettlement { get; set; }
    }
}
