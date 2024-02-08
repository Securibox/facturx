namespace Securibox.FacturX.Models.Minimum
{
    public class Invoice
    {
        public ProccessControl ProccessControl { get; set; }
        public Header Header { get; set; }
        public StakeHolders StakeHolders { get; set; }
        public References References { get; set; }
        public DirectDebit DirectDebit { get; set; }
        public Totals Totals { get; set; }
    }
}
