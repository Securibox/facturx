namespace Securibox.FacturX.Models.BasicWL
{
    public class Invoice : Minimum.Invoice
    {
        public new Header Header { get; set; }

        public new StakeHolders StakeHolders { get; set; }

        public new References? References { get;    set; }

        public IList<AllowanceCharge>? AllowanceList { get; set; }

        public IList<AllowanceCharge>? ChargeList { get; set; }

        public IEnumerable<TaxDistribution>? TaxDistributionList { get; set; }

        public new Totals Totals { get; set; }

        public PaymentTerms? PaymentTerms { get; set; }

        public PaymentInstructions? PaymentInstructions { get; set; }

        public new DirectDebit DirectDebit { get; set; }

        public DeliveryDetails? DeliveryDetails { get; set; }
    }
}
