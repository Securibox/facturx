namespace Securibox.FacturX.Models.EN16931
{
    public class Buyer : BasicWL.Buyer
    {
        public new BasicWL.LegalRegistration? LegalRegistration { get; internal set; }

        public Contact? Contact { get; internal set; }
    }
}