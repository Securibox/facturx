namespace Securibox.FacturX.Models.Extended
{
    public class Buyer : EN16931.Buyer
    {
        public new LegalRegistration? LegalRegistration { get; internal set; }

        public new Contact? Contact { get; internal set; }

        public string AdditionalLegalInformation { get; internal set; }
    }
}