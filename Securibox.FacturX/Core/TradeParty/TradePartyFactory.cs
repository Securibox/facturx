using System.Xml;
using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Models;

namespace Securibox.FacturX.Core
{
    internal class TradePartyFactory
    {
        private readonly ActorConverter _actorConverter;

        internal TradePartyFactory(FacturXConformanceLevelType conformanceLevelType, XmlDocument xmlDocument)
        {
            _actorConverter = new ActorConverter(conformanceLevelType, xmlDocument);
        }

        internal IActor? ConvertActor(TradePartyType type)
        {
            switch (type)
            {
                case TradePartyType.Seller:
                    return _actorConverter.GetSeller();

                case TradePartyType.Buyer:
                    return _actorConverter.GetBuyer();

                case TradePartyType.SalesAgent:
                    return _actorConverter.GetSalesAgent();

                case TradePartyType.BuyerAgent:
                    return _actorConverter.GetBuyerAgent();

                case TradePartyType.ProductEndUser:
                    return _actorConverter.GetProductEndUser();

                case TradePartyType.BuyerTaxRepresentative:
                    return _actorConverter.GetBuyerTaxRepresentative();

                case TradePartyType.SellerTaxRepresentative:
                    return _actorConverter.GetSellerTaxRepresentative();

                case TradePartyType.DeliverToAddress:
                    return _actorConverter.GetDeliverToAddress();

                case TradePartyType.DeliverFromAddress:
                    return _actorConverter.GetDeliverFromAddress();

                case TradePartyType.UltimateDeliverToAddress:
                    return _actorConverter.GetUltimateDeliverToAddress();

                case TradePartyType.Invoicer:
                    return _actorConverter.GetInvoicer();

                case TradePartyType.Invoicee:
                    return _actorConverter.GetInvoicee();

                case TradePartyType.Payee:
                    return _actorConverter.GetPayee();

                case TradePartyType.Payer:
                    return _actorConverter.GetPayer();

                case TradePartyType.PayeeMultiplePaymentsAndPayee:
                    return _actorConverter.GetPayeeMultiplePayment();

                default:
                    return null;

            }
        }

        internal IActor? ConvertLineActor(XmlNode lineNode, TradePartyType type)
        {
            switch (type)
            {
                case TradePartyType.DeliverToAddress:
                    return _actorConverter.GetLineDeliverToAddress(lineNode);

                case TradePartyType.UltimateDeliverToAddress:
                    return _actorConverter.GetLineUltimateDeliverToAddress(lineNode);

                default:
                    return null;

            }
        }
    }
}
