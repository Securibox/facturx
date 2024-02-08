namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class LegalOrganization
    {
        public Minimum.ID ID { get; set; }
        public string TradingBusinessName { get; set; }
        public TradeAddressExtended PostalTradeAddress { get; set; }
    }
}
