namespace Securibox.FacturX.Models.BasicWL
{
    public class DeliverToAddress : IActor
    {
        public string? Id { get; internal set; }

        public GlobalIdentification? GlobalIdentification { get; internal set; }

        public string? Name { get; internal set; }

        public BasicWL.PostalAddress? PostalAddress { get; internal set; }
    }
}
