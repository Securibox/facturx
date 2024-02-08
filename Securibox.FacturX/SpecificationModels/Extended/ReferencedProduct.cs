using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class ReferencedProduct
    {
        public Minimum.ID ID { get; set; }
        [XmlElement]
        public Minimum.ID[] GlobalID { get; set; }
        public Minimum.ID SellerAssignedID { get; set; }
        public Minimum.ID BuyerAssignedID { get; set; }
        public Minimum.ID IndustryAssignedID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Basic.Quantity UnitQuantity { get; set; }
    }
}
