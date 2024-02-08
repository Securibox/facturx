namespace Securibox.FacturX.SpecificationModels.Minimum
{
    public class TradeParty
    {
        public string Name { get; set; }
        public LegalOrganization SpecifiedLegalOrganization { get; set; }
        public TradeAddress PostalTradeAddress { get; set; }
        public TaxRegistration SpecifiedTaxRegistration { get; set; }
    }
}
