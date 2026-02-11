using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class TradeProduct
    {
        public string ID { get; set; }
        public Minimum.ID GlobalID { get; set; }
        public string SellerAssignedID { get; set; }
        public string BuyerAssignedID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [XmlElement]
        public ProductCharacteristic[] ApplicableProductCharacteristic { get; set; }

        [XmlElement]
        public ProductClassification[] DesignatedProductClassification { get; set; }

        [XmlElement]
        public TradeProductInstance[] IndividualTradeProductInstance { get; set; }
        public TradeCountry OriginTradeCountry { get; set; }

        [XmlElement]
        public ReferencedProduct[] IncludedReferencedProduct { get; set; }
    }
}
