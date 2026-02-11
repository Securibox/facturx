namespace Securibox.FacturX.Models.Extended
{
    public class InvoiceLine : EN16931.InvoiceLine
    {
        public new LineDetails LineDetails { get; internal set; }

        public new LineItemDetails ItemDetails { get; internal set; }

        public new LineGrossPriceDetails? GrossPriceDetails { get; internal set; }

        public new LineNetPriceDetails? NetPriceDetails { get; internal set; }

        public new LineDeliveryDetails DeliveryDetails { get; internal set; }

        public new LineVatDetails? VatDetails { get; internal set; }

        public new LineTotals Totals { get; internal set; }

        public new IEnumerable<Extended.LineItemAttribute>? ItemAttributeList { get; internal set; }

        public new IEnumerable<Extended.LineItemClassification>? ItemClassificationList
        {
            get;
            internal set;
        }

        public IEnumerable<Extended.LineItemInstance>? ItemInstanceList { get; internal set; }

        public new LinePurchaseOrderReference? PurchaseOrderReference { get; internal set; }

        public Extended.LineQuotationReference? QuotationReference { get; internal set; }

        public Extended.LineContractReference? ContractReference { get; internal set; }

        public IEnumerable<LineIncludedItem>? IncludedItemList { get; internal set; }

        public IEnumerable<LineAdditionalDocument>? AdditionalDocumentReferenceList
        {
            get;
            internal set;
        }

        public IEnumerable<LineUltimateCustomerOrderReference>? UltimateCustomerOrderReferenceList
        {
            get;
            internal set;
        }
    }
}
