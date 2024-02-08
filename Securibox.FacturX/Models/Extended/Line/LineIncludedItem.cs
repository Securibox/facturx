using Securibox.FacturX.Models.Basic;
using Securibox.FacturX.Models.BasicWL;

namespace Securibox.FacturX.Models.Extended
{
    public class LineIncludedItem
    {
        public string? Id { get; private set; }
        public IEnumerable<GlobalIdentification>? GlobalIdentificationList { get; private set; }
        public string? SellerAssignedId { get; private set; }
        public string? BuyerAssignedId { get; private set; }
        public string? IndustryAssignedId { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public QuantityUnit? UnitQuantity { get; private set; }

        internal LineIncludedItem(string name) => Name = name;

        internal void AddId(string? id) => Id = id;
        internal void AddGlobalIdentificationList(IEnumerable<GlobalIdentification> globalIdentificationList) => GlobalIdentificationList = globalIdentificationList;
        internal void AddSellerAssignedId(string? sellerAssignedId) => SellerAssignedId = sellerAssignedId;
        internal void AddBuyerAssignedId(string? buyerAssignedId) => BuyerAssignedId = buyerAssignedId;
        internal void AddIndustryAssignedId(string? industryAssignedId) => IndustryAssignedId = industryAssignedId;
        internal void AddDescription(string? description) => Description = description;
        internal void AddUnitQuantity(QuantityUnit? unitQuantity) => UnitQuantity = unitQuantity;
    }
}
