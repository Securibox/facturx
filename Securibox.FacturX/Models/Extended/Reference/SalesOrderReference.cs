namespace Securibox.FacturX.Models.Extended
{
    public class SalesOrderReference : EN16931.SalesOrderReference
    {
        public DateTime? IssueDate { get; internal set; }
    }
}
