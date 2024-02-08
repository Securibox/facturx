namespace Securibox.FacturX.Models.BasicWL
{
    public class CreditTransfer
    {
        public string IbanId { get; private set; }
        public string? ProprietaryId { get; private set; }

        public CreditTransfer(string ibanId, string? proprietaryId)
        {
            IbanId = ibanId;
            ProprietaryId = proprietaryId;
        }
    }
}
