namespace Securibox.FacturX.Models.Extended
{
    public class References : EN16931.References
    {
        public new SalesOrderReference? SalesOrderReference { get; internal set; }

        public new PurchaseOrderReference? PurchaseOrderReference { get; internal set; }

        public QuotationReference? QuotationReference { get; internal set; }

        public new ContractReference? ContractReference { get; internal set; }

        public new IEnumerable<SupportingDocument>? SupportingDocumentList { get; internal set; }

        public new TenderOrLotReference? TenderOrLotReference { get; internal set; }

        public new InvoicedObjectIdentifier? InvoicedObjectIdentifier { get; internal set; }

        public IEnumerable<UltimateCustomerOrderReference>? UltimateCustomerOrderReferenceList { get; internal set; }

        public new Models.Extended.BuyerAccountingReference? BuyerAccountingReference { get; internal set; }

        public new IEnumerable<Models.Extended.PreviousInvoiceReference>? PreviousInvoiceReferenceList { get; internal set; }
    }
}