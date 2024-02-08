namespace Securibox.FacturX.Models.EN16931
{
    public class CardInformation
    {
        public string PrimaryAccountNumber { get; private set; }
        public string? CardHolderName { get; private set; }

        public CardInformation(string primaryAccountNumber, string? cardHolderName) 
        {
            PrimaryAccountNumber = primaryAccountNumber;
            CardHolderName = cardHolderName;
        }
    }
}
