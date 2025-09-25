namespace Securibox.FacturX.Models.EN16931
{
    public class References : BasicWL.References
    {
        public SalesOrderReference? SalesOrderReference { get; internal set; }

        public IEnumerable<SupportingDocument>? SupportingDocumentList { get; internal set; }

        public TenderOrLotReference? TenderOrLotReference { get; internal set; }

        public InvoicedObjectIdentifier? InvoicedObjectIdentifier { get; internal set; }

        public ProjectReference? ProjectReference { get; internal set; }
    }
}