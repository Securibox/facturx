namespace Securibox.FacturX.Models.EN16931
{
    public class InvoiceLine : Models.Basic.InvoiceLine
    {
        public new LineItemDetails ItemDetails { get; internal set; }

        public IEnumerable<LineItemAttribute>? ItemAttributeList { get; internal set; }

        public IEnumerable<LineItemClassification>? ItemClassificationList { get; internal set; }

        public Models.EN16931.InvoicedObjectIdentifier? InvoicedObjectIdentifier { get; internal set; }

        public Models.BasicWL.BuyerAccountingReference? BuyerAccountingReference { get; internal set; }

        public LinePurchaseOrderReference? PurchaseOrderReference { get; internal set; }

        public new IEnumerable<LineAllowanceCharge>? ChargeList { get; internal set; }

        public new IEnumerable<LineAllowanceCharge>? AllowanceList { get; internal set; }
    }
}
