namespace Securibox.FacturX.Models.BasicWL
{
    public class References : Minimum.References
    {
        public ContractReference? ContractReference { get; internal set; }

        public BuyerAccountingReference? BuyerAccountingReference { get; internal set; }

        public IEnumerable<PreviousInvoiceReference>? PreviousInvoiceReferenceList { get; internal set; }
    }
}