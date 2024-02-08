namespace Securibox.FacturX.Models.EN16931
{
    public class DeliveryDetails : BasicWL.DeliveryDetails
    {
        public ReceivingAdviceReference? ReceivingAdviceReference { get; internal set; }
    }
}