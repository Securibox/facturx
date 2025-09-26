namespace Securibox.FacturX.Models.Extended
{
    public class Seller : EN16931.Seller
    {
        public new Contact? Contact { get; internal set; }

        public new LegalRegistration? LegalRegistration { get; internal set; }
    }
}