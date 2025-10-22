namespace Securibox.FacturX.Models.EN16931
{
    public class StakeHolders : BasicWL.StakeHolders
    {
        public new Seller? Seller { get; internal set; }

        public new Buyer? Buyer { get; internal set; }
    }
}
