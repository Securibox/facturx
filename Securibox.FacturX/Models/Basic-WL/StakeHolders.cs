namespace Securibox.FacturX.Models.BasicWL
{
    public class StakeHolders : Minimum.StakeHolders
    {
        public new Models.BasicWL.Seller? Seller { get; set; }

        public new Models.BasicWL.Buyer? Buyer { get; set; }

        public SellerTaxRepresentative? SellerTaxRepresentative { get; set; }

        public BasicWL.PaymentActor? Payee { get; set; }
    }
}
