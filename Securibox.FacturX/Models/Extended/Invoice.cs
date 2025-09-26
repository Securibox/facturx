namespace Securibox.FacturX.Models.Extended
{
    public class Invoice : EN16931.Invoice
    {
        public new ProccessControl ProccessControl { get; internal set; }

        public new Header Header { get; internal set; }

        public new StakeHolders StakeHolders { get; internal set; }

        public new References? References { get; internal set; }

        public new IEnumerable<TaxDistribution>? TaxDistributionList { get; internal set; }

        public new Totals Totals { get; internal set; }

        public DeliveryDetails? DeliveryDetails { get; internal set; }

        public new IEnumerable<Extended.InvoiceLine>? LineList { get; internal set; }

        public new IEnumerable<AllowanceCharge>? AllowanceList { get; internal set; }

        public new IEnumerable<AllowanceCharge>? ChargeList { get; internal set; }

        public IList<LogisticsServiceCharge>? LogisticsServiceChargeList { get; internal set; }

        public new DirectDebit DirectDebit { get; internal set; }

        public new PaymentTerms? PaymentTerms { get; internal set; }

        public TaxCurrencyExchange? TaxCurrencyExchange { get; internal set; }

        public IEnumerable<AdvancePayment>? AdvancePaymentList { get; internal set; }
    }
}