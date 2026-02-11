namespace Securibox.FacturX.Models.EN16931
{
    public class Invoice : Basic.Invoice
    {
        public new StakeHolders? StakeHolders { get; internal set; }

        public new References? References { get; internal set; }

        public new IEnumerable<TaxDistribution>? TaxDistributionList { get; internal set; }

        public new Totals? Totals { get; internal set; }

        public new PaymentInstructions? PaymentInstructions { get; internal set; }

        public DeliveryDetails? DeliveryDetails { get; internal set; }

        public new IEnumerable<EN16931.InvoiceLine>? LineList { get; internal set; }
    }
}
