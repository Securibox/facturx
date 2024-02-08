namespace Securibox.FacturX.SpecificationModels.Extended
{
    internal class CreditorFinancialAccount
    {
        public Minimum.ID IBANID { get; set; }
        public string AccountName { get; set; }
        public Minimum.ID ProprietaryID { get; set; }
    }
}
