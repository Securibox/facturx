namespace Securibox.FacturX.SpecificationModels.EN16931
{
    public class LineTradeAgreement
    {
        public ReferencedDocumentEN16931 BuyerOrderReferencedDocument { get; set; }
        public Basic.TradePrice GrossPriceProductTradePrice { get; set; }
        public Basic.TradePrice NetPriceProductTradePrice { get; set; }
    }
}