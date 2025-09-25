namespace Securibox.FacturX.Models.Extended
{
    public class StakeHolders : BasicWL.StakeHolders
    {
        public new Seller? Seller { get; internal set; }

        public new Buyer? Buyer { get; internal set; }

        public new SellerTaxRepresentative? SellerTaxRepresentative { get; internal set; }

        public SalesAgent? SalesAgent { get; internal set; }

        public BuyerAgent? BuyerAgent { get; internal set; }

        public BuyerTaxRepresentative? BuyerTaxRepresentative { get; internal set; }

        public ProductEndUser? ProductEndUser { get; internal set; }

        public new Extended.PaymentActor? Payee { get; internal set; }

        public Extended.PaymentActor? PayeeMultiplePayment { get; internal set; }

        public Extended.PaymentActor? Payer { get; internal set; }

        public Extended.Invoicer? Invoicer { get; internal set; }

        public Extended.Invoicee? Invoicee { get; internal set; }
    }
}