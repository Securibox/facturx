namespace Securibox.FacturX.Models.EN16931
{
    public class LinePurchaseOrderReference : IReference
    {
        public string? LineId { get; private set; }

        internal LinePurchaseOrderReference(string? lineId)
        {
            LineId = lineId;
        }
    }
}
