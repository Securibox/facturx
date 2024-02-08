using Securibox.FacturX.Models.BasicWL;

namespace Securibox.FacturX.Models.EN16931
{
    public class LineItemDetails : Basic.LineItemDetails
    {
        public string? SellerAssignedId { get; private set; }
        public string? BuyerAssignedId { get; private set; }
        public string? Description { get; private set; }
        public string? OriginCountry { get; private set; }

        public LineItemDetails(string name, GlobalIdentification? standardIdentification) 
            : base(name, standardIdentification) { }

        internal void AddSellerAssignedId(string? sellerAssignedId) => SellerAssignedId = sellerAssignedId;
        internal void AddBuyerAssignedId(string? buyerAssignedId) => BuyerAssignedId = buyerAssignedId;
        internal void AddDescription(string? description) => Description = description;
        internal void AddOriginCountry(string? originCountry) => OriginCountry = originCountry; 
    }
}
