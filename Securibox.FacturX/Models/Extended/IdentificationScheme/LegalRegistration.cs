namespace Securibox.FacturX.Models.Extended
{
    public class LegalRegistration : BasicWL.LegalRegistration
    {
        public BasicWL.PostalAddress? PostalAddress { get; internal set; }
    }
}
