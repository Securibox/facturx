namespace Securibox.FacturX.Models.BasicWL
{
    public class PaymentActor : IActor
    {
        public string? Id { get; internal set; }

        public GlobalIdentification? GlobalIdentification { get; internal set; }

        public string? Name { get; internal set; }

        public Minimum.LegalRegistration? LegalRegistration { get; internal set; }
    }
}
