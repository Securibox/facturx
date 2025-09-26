namespace Securibox.FacturX.Models.Basic
{
    public class Invoice : BasicWL.Invoice
    {
        public virtual IEnumerable<Basic.InvoiceLine> LineList { get; set; }
    }
}