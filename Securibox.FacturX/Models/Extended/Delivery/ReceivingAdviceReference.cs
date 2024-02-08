namespace Securibox.FacturX.Models.Extended
{
    public class ReceivingAdviceReference : EN16931.ReceivingAdviceReference
    {
        public DateTime? IssueDate { get; internal set; }
    }
}