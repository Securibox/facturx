using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.EN16931
{
    public class TradeProduct
    {
        public Minimum.ID GlobalID { get; set; }
        public string SellerAssignedID { get; set; }
        public string BuyerAssignedID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [XmlElement]
        public ProductCharacteristic[] ApplicableProductCharacteristic { get; set; }
        [XmlElement]
        public ProductClassification[] DesignatedProductClassification { get; set; }
        public TradeCountry OriginTradeCountry { get; set; }
    }
}