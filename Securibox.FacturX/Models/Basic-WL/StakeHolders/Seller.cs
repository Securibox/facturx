namespace Securibox.FacturX.Models.BasicWL
{
    public class Seller : Minimum.Seller
    {
        public new LegalRegistration? LegalRegistration { get; set; }
        public new PostalAddress? PostalAddress { get; set; }
        public IEnumerable<string>? IdList { get; set; }
        public IEnumerable<GlobalIdentification?>? GlobalIdentificationList { get; set; }
        public ElectronicAddress? ElectronicAddress { get; set; }
    }
}