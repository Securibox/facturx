using System.Xml;
using Securibox.FacturX.Models;
using Securibox.FacturX.Models.Enums;
using Securibox.FacturX.Models.Extended;

namespace Securibox.FacturX.Core
{
    internal class ActorConverter : TradePartyConverter
    {
        private XmlNode _buyerNode;
        private XmlNode _sellerNode;
        private XmlNode? _buyerAgentNode;
        private XmlNode? _salesAgentNode;
        private XmlNode? _buyerTaxRepresentativeNode;
        private XmlNode? _sellerTaxRepresentativeNode;

        private XmlNode? _invoiceeNode;
        private XmlNode? _invoicerNode;
        private XmlNode? _payeeNode;
        private XmlNode? _payerNode;
        private XmlNode? _payeeMultiplePaymentNode;
        private XmlNode? _productEndUserNode;

        private XmlNode? _deliverToAddressNode;
        private XmlNode? _deliverFromAddressNode;
        private XmlNode? _ultimateDeliverToAddressNode;

        internal ActorConverter(
            FacturXConformanceLevelType conformanceLevelType,
            XmlDocument xmlDocument
        )
            : base(conformanceLevelType)
        {
            var tradeAgreementNode = xmlDocument.SelectSingleNode(
                "//*[local-name() = 'ApplicableHeaderTradeAgreement']"
            )!;
            _buyerNode = tradeAgreementNode.SelectSingleNode(
                "*[local-name() = 'BuyerTradeParty']"
            )!;
            _sellerNode = tradeAgreementNode.SelectSingleNode(
                "*[local-name() = 'SellerTradeParty']"
            )!;

            _buyerAgentNode = tradeAgreementNode.SelectSingleNode(
                "*[local-name() = 'BuyerAgentTradeParty']"
            );
            _salesAgentNode = tradeAgreementNode.SelectSingleNode(
                "*[local-name() = 'SalesAgentTradeParty']"
            );

            _buyerTaxRepresentativeNode = tradeAgreementNode.SelectSingleNode(
                "*[local-name() = 'BuyerTaxRepresentativeTradeParty']"
            );
            _sellerTaxRepresentativeNode = tradeAgreementNode.SelectSingleNode(
                "*[local-name() = 'SellerTaxRepresentativeTradeParty']"
            );

            _productEndUserNode = tradeAgreementNode.SelectSingleNode(
                "*[local-name() = 'ProductEndUserTradeParty']"
            );

            var tradeSettlementNode = xmlDocument.SelectSingleNode(
                "//*[local-name() = 'ApplicableHeaderTradeSettlement']"
            )!;
            _invoiceeNode = tradeSettlementNode.SelectSingleNode(
                "*[local-name() = 'InvoiceeTradeParty']"
            );
            _invoicerNode = tradeSettlementNode.SelectSingleNode(
                "*[local-name() = 'InvoicerTradeParty']"
            );
            _payeeNode = tradeSettlementNode.SelectSingleNode(
                "*[local-name() = 'PayeeTradeParty']"
            );
            _payerNode = tradeSettlementNode.SelectSingleNode(
                "*[local-name() = 'PayerTradeParty']"
            );

            _payeeMultiplePaymentNode = tradeSettlementNode
                .SelectSingleNode("*[local-name() = 'SpecifiedTradePaymentTerms']")
                ?.SelectSingleNode("*[local-name() = 'PayeeTradeParty']");

            var tradeDeliveryNode = xmlDocument.SelectSingleNode(
                "//*[local-name() = 'ApplicableHeaderTradeDelivery']"
            );

            _deliverFromAddressNode = tradeDeliveryNode?.SelectSingleNode(
                "*[local-name() = 'ShipFromTradeParty']"
            );
            _deliverToAddressNode = tradeDeliveryNode?.SelectSingleNode(
                "*[local-name() = 'ShipToTradeParty']"
            );
            _ultimateDeliverToAddressNode = tradeDeliveryNode?.SelectSingleNode(
                "*[local-name() = 'UltimateShipToTradeParty']"
            );
        }

        internal Models.Minimum.Buyer? GetBuyer()
        {
            var name = GetName(_buyerNode);

            var legalRegistration = GetLegalRegistration(_buyerNode);

            if (ConformanceLevelType == FacturXConformanceLevelType.Minimum)
            {
                return new Models.Minimum.Buyer
                {
                    Name = name,
                    LegalRegistration = legalRegistration as Models.Minimum.LegalRegistration,
                };
            }

            var postalAddress = GetPostalAddress(_buyerNode);

            var id = GetIdList(_buyerNode)?.FirstOrDefault();
            var globalIdentification = GetActorGlobalIdentification(_buyerNode);
            var vatRegistration = GetVatRegistration(_buyerNode);
            var electronicAddress = GetElectronicAddress(_buyerNode);

            if (
                ConformanceLevelType == FacturXConformanceLevelType.BasicWL
                || ConformanceLevelType == FacturXConformanceLevelType.Basic
            )
            {
                return new Models.BasicWL.Buyer
                {
                    Name = name,
                    LegalRegistration = legalRegistration as Models.Minimum.LegalRegistration,
                    Id = id,
                    GlobalIdentification = globalIdentification,
                    ElectronicAddress = electronicAddress,
                    PostalAddress = postalAddress as Models.BasicWL.PostalAddress,
                    VatRegistration = vatRegistration,
                };
            }

            var contact = GetContact(_buyerNode);

            if (ConformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                return new Models.EN16931.Buyer
                {
                    Name = name,
                    LegalRegistration = legalRegistration as Models.BasicWL.LegalRegistration,
                    Id = id,
                    GlobalIdentification = globalIdentification,
                    ElectronicAddress = electronicAddress,
                    PostalAddress = postalAddress as Models.BasicWL.PostalAddress,
                    VatRegistration = vatRegistration,
                    Contact = contact,
                };
            }

            return new Models.Extended.Buyer
            {
                Name = name,
                LegalRegistration = legalRegistration as Models.Extended.LegalRegistration,
                Id = id,
                GlobalIdentification = globalIdentification,
                ElectronicAddress = electronicAddress,
                PostalAddress = postalAddress as Models.BasicWL.PostalAddress,
                VatRegistration = vatRegistration,
                Contact = contact as Models.Extended.Contact,
                AdditionalLegalInformation = _buyerNode
                    .SelectSingleNode("*[local-name() = 'Description']")
                    ?.InnerText,
            };
        }

        internal Models.Extended.BuyerAgent? GetBuyerAgent()
        {
            if (_buyerAgentNode == null)
                return null;

            var name = GetName(_buyerAgentNode);
            var id = GetIdList(_buyerAgentNode)?.FirstOrDefault();
            var globalIdentificationList = GetGlobalIdentificationList(_buyerAgentNode);
            var legalRegistration =
                GetLegalRegistration(_buyerAgentNode) as Models.Extended.LegalRegistration;
            var contact = GetContact(_buyerAgentNode) as Contact;
            var postalAddress = GetPostalAddress(_buyerAgentNode) as Models.BasicWL.PostalAddress;

            var electronicAddress = GetElectronicAddress(_buyerAgentNode);
            var vatRegistration = GetVatRegistration(_buyerAgentNode);

            return new BuyerAgent
            {
                Id = id,
                GlobalIdentificationList = globalIdentificationList,
                Name = name,
                LegalRegistration = legalRegistration,
                Contact = contact,
                PostalAddress = postalAddress,
                ElectronicAddress = electronicAddress,
                VatRegistration = vatRegistration,
            };
        }

        internal BuyerTaxRepresentative? GetBuyerTaxRepresentative()
        {
            if (_buyerTaxRepresentativeNode == null)
                return null;

            var name = GetName(_buyerTaxRepresentativeNode);
            var id = GetIdList(_buyerTaxRepresentativeNode)?.FirstOrDefault();
            var globalIdentificationList = GetGlobalIdentificationList(_buyerTaxRepresentativeNode);
            var legalRegistration =
                GetLegalRegistration(_buyerTaxRepresentativeNode)
                as Models.Extended.LegalRegistration;
            var contact = GetContact(_buyerTaxRepresentativeNode) as Contact;

            var postalAddress =
                GetPostalAddress(_buyerTaxRepresentativeNode) as Models.BasicWL.PostalAddress;

            var electronicAddress = GetElectronicAddress(_buyerTaxRepresentativeNode);
            var vatRegistration = GetVatRegistration(_buyerTaxRepresentativeNode);

            return new BuyerTaxRepresentative
            {
                Id = id,
                GlobalIdentificationList = globalIdentificationList,
                Name = name,
                LegalRegistration = legalRegistration,
                Contact = contact,
                ElectronicAddress = electronicAddress,
                VatRegistration = vatRegistration,
                PostalAddress = postalAddress,
            };
        }

        internal Models.BasicWL.DeliverToAddress? GetDeliverToAddress()
        {
            if (_deliverToAddressNode == null)
                return null;

            var id = GetId(_deliverToAddressNode);
            var globalIdentification = GetGlobalIdOptionalScheme(_deliverToAddressNode);
            var name = GetName(_deliverToAddressNode);
            var postalAddress =
                GetPostalAddress(_deliverToAddressNode) as Models.BasicWL.PostalAddress;

            if (
                ConformanceLevelType == FacturXConformanceLevelType.BasicWL
                || ConformanceLevelType == FacturXConformanceLevelType.Basic
                || ConformanceLevelType == FacturXConformanceLevelType.EN16931
            )
            {
                return new DeliverToAddress
                {
                    Id = id,
                    GlobalIdentification = globalIdentification,
                    Name = name,
                    PostalAddress = postalAddress,
                };
            }

            var legalRegistration = GetLegalRegistration(_deliverToAddressNode);
            var contact = GetContact(_deliverToAddressNode);
            var electronicAddress = GetElectronicAddress(_deliverToAddressNode);
            var vatRegistrationList = GetVatRegistrationList(_deliverToAddressNode);

            return new Models.Extended.DeliverToAddress
            {
                Id = id,
                GlobalIdentification = globalIdentification,
                Name = name,
                PostalAddress = postalAddress,
                LegalRegistration = legalRegistration,
                Contact = contact,
                ElectronicAddress = electronicAddress,
                VatRegistrationList = vatRegistrationList,
            };
        }

        internal DeliverFromAddress? GetDeliverFromAddress()
        {
            if (_deliverFromAddressNode == null)
                return null;

            var id = GetIdList(_deliverFromAddressNode)?.FirstOrDefault();
            var globalIdentificationList = GetGlobalIdentificationList(_deliverFromAddressNode);
            var name = GetName(_deliverFromAddressNode);

            var postalAddress =
                GetPostalAddress(_deliverFromAddressNode) as Models.BasicWL.PostalAddress;

            var legalRegistration =
                GetLegalRegistration(_deliverFromAddressNode) as Models.Extended.LegalRegistration;
            var contact = GetContact(_deliverFromAddressNode) as Contact;
            var electronicAddress = GetElectronicAddress(_deliverFromAddressNode);
            var vatRegistrationList = GetVatRegistrationList(_deliverFromAddressNode);

            return new DeliverFromAddress
            {
                Id = id,
                GlobalIdentificationList = globalIdentificationList,
                Name = name,
                PostalAddress = postalAddress,
                LegalRegistration = legalRegistration,
                Contact = contact,
                ElectronicAddress = electronicAddress,
                VatRegistrationList = vatRegistrationList,
            };
        }

        internal Invoicee? GetInvoicee()
        {
            if (_invoiceeNode == null)
                return null;

            var id = GetIdList(_invoiceeNode)?.FirstOrDefault();
            var globalIdentificationList = GetGlobalIdentificationList(_invoiceeNode);
            var name = GetName(_invoiceeNode);
            var legalRegistration =
                GetLegalRegistration(_invoiceeNode) as Models.Extended.LegalRegistration;
            var contact = GetContact(_invoiceeNode) as Contact;

            var postalAddress = GetPostalAddress(_invoiceeNode) as Models.BasicWL.PostalAddress;

            var electronicAddress = GetElectronicAddress(_invoiceeNode);
            var vatRegistration = GetVatRegistration(_invoiceeNode);

            return new Invoicee
            {
                Id = id,
                GlobalIdentificationList = globalIdentificationList,
                Name = name,
                LegalRegistration = legalRegistration,
                Contact = contact,
                PostalAddress = postalAddress,
                ElectronicAddress = electronicAddress,
                VatRegistration = vatRegistration,
            };
        }

        internal Models.Extended.Invoicer? GetInvoicer()
        {
            if (_invoicerNode == null)
                return null;

            var id = GetIdList(_invoicerNode)?.FirstOrDefault();
            var globalIdentificationList = GetGlobalIdentificationList(_invoicerNode);
            var name = GetName(_invoicerNode);
            var legalRegistration =
                GetLegalRegistration(_invoicerNode) as Models.Extended.LegalRegistration;
            var contact = GetContact(_invoicerNode) as Contact;

            var postalAddress = GetPostalAddress(_invoicerNode) as Models.BasicWL.PostalAddress;

            var electronicAddress = GetElectronicAddress(_invoicerNode);
            var vatRegistration = GetVatRegistration(_invoicerNode);

            return new Invoicer
            {
                Id = id,
                GlobalIdentificationList = globalIdentificationList,
                Name = name,
                LegalRegistration = legalRegistration,
                Contact = contact,
                PostalAddress = postalAddress,
                ElectronicAddress = electronicAddress,
                VatRegistration = vatRegistration,
            };
        }

        internal UltimateDeliverToAddress? GetUltimateDeliverToAddress()
        {
            if (_ultimateDeliverToAddressNode == null)
                return null;

            var id = GetIdList(_ultimateDeliverToAddressNode)?.FirstOrDefault();
            var globalIdentificationList = GetGlobalIdentificationList(
                _ultimateDeliverToAddressNode
            );
            var name = GetName(_ultimateDeliverToAddressNode);

            var postalAddress =
                GetPostalAddress(_ultimateDeliverToAddressNode) as Models.BasicWL.PostalAddress;

            var legalRegistration =
                GetLegalRegistration(_ultimateDeliverToAddressNode)
                as Models.Extended.LegalRegistration;
            var contact = GetContact(_ultimateDeliverToAddressNode) as Contact;
            var electronicAddress = GetElectronicAddress(_ultimateDeliverToAddressNode);
            var vatRegistrationList = GetVatRegistrationList(_ultimateDeliverToAddressNode);

            return new UltimateDeliverToAddress
            {
                Id = id,
                GlobalIdentificationList = globalIdentificationList,
                Name = name,
                PostalAddress = postalAddress,
                LegalRegistration = legalRegistration,
                Contact = contact,
                ElectronicAddress = electronicAddress,
                VatRegistrationList = vatRegistrationList,
            };
        }

        internal Models.BasicWL.PaymentActor? GetPayee()
        {
            if (_payeeNode == null)
                return null;

            var name = GetName(_payeeNode);
            var id = GetIdList(_payeeNode)?.FirstOrDefault();
            var globalIdentification = GetActorGlobalIdentification(_payeeNode);
            var legalRegistration = GetLegalRegistration(_payeeNode);

            var basicWLPayee = new Models.BasicWL.PaymentActor
            {
                Id = id,
                GlobalIdentification = globalIdentification,
                Name = name,
                LegalRegistration = legalRegistration as Models.BasicWL.LegalRegistration,
            };

            if (ConformanceLevelType == FacturXConformanceLevelType.Extended)
            {
                var vatRegistrationList = GetVatRegistrationList(_payeeNode);

                var postalAddress = GetPostalAddress(_payeeNode) as Models.BasicWL.PostalAddress;

                var electronicAddress = GetElectronicAddress(_payeeNode);
                var contact = GetContact(_payeeNode) as Contact;
                var roleCode = _payeeNode
                    .SelectSingleNode("*[local-name() = 'RoleCode']")
                    ?.InnerText;

                return new Models.Extended.PaymentActor
                {
                    Id = id,
                    GlobalIdentification = globalIdentification,
                    Name = name,
                    LegalRegistration = legalRegistration as Models.Extended.LegalRegistration,
                    PostalAddress = postalAddress,
                    ElectronicAddress = electronicAddress,
                    Contact = contact,
                    RoleCode = roleCode,
                    VatRegistrationList = vatRegistrationList,
                };
            }

            return basicWLPayee;
        }

        internal Models.Extended.PaymentActor? GetPayeeMultiplePayment()
        {
            if (_payeeMultiplePaymentNode == null)
                return null;

            var name = GetName(_payeeMultiplePaymentNode);

            var id = GetIdList(_payeeMultiplePaymentNode)?.FirstOrDefault();
            var globalIdentification = GetActorGlobalIdentification(_payeeMultiplePaymentNode);
            var legalRegistration = GetLegalRegistration(_payeeMultiplePaymentNode);
            var roleCode = _payeeMultiplePaymentNode
                .SelectSingleNode("*[local-name() = 'RoleCode']")
                ?.InnerText;
            var contact = GetContact(_payeeMultiplePaymentNode);

            var postalAddress =
                GetPostalAddress(_payeeMultiplePaymentNode) as Models.BasicWL.PostalAddress;

            var electronicAddress = GetElectronicAddress(_payeeMultiplePaymentNode);
            var vatRegistartionList = GetVatRegistrationList(_payeeMultiplePaymentNode);

            return new Models.Extended.PaymentActor
            {
                Name = name,
                Id = id,
                GlobalIdentification = globalIdentification,
                LegalRegistration = legalRegistration,
                RoleCode = roleCode,
                Contact = contact,
                PostalAddress = postalAddress,
                ElectronicAddress = electronicAddress,
                VatRegistrationList = vatRegistartionList,
            };
        }

        internal Models.Extended.PaymentActor? GetPayer()
        {
            if (_payerNode == null)
                return null;

            var name = GetName(_payerNode);
            var id = GetIdList(_payerNode)?.FirstOrDefault();
            var globalIdentification = GetActorGlobalIdentification(_payerNode);
            var legalRegistration = GetLegalRegistration(_payerNode);
            var roleCode = _payerNode.SelectSingleNode("*[local-name() = 'RoleCode']")?.InnerText;
            var contact = GetContact(_payerNode);

            var postalAddress = GetPostalAddress(_payerNode) as Models.BasicWL.PostalAddress;

            var electronicAddress = GetElectronicAddress(_payerNode);
            var vatRegistartionList = GetVatRegistrationList(_payerNode);

            return new Models.Extended.PaymentActor
            {
                Name = name,
                Id = id,
                GlobalIdentification = globalIdentification,
                LegalRegistration = legalRegistration,
                RoleCode = roleCode,
                Contact = contact,
                PostalAddress = postalAddress,
                ElectronicAddress = electronicAddress,
                VatRegistrationList = vatRegistartionList,
            };
        }

        internal Models.Extended.ProductEndUser? GetProductEndUser()
        {
            if (_productEndUserNode == null)
                return null;

            var name = GetName(_productEndUserNode);
            var id = GetIdList(_productEndUserNode)?.FirstOrDefault();
            var globalIdentificationList = GetGlobalIdentificationList(_productEndUserNode);
            var legalRegistration =
                GetLegalRegistration(_productEndUserNode) as Models.Extended.LegalRegistration;
            var contact = GetContact(_productEndUserNode) as Models.Extended.Contact;

            var postalAddress = GetPostalAddress(_productEndUserNode);

            var electronicAddress = GetElectronicAddress(_productEndUserNode);
            var vatRegistrationList = GetVatRegistrationList(_productEndUserNode);

            return new ProductEndUser
            {
                Id = id,
                GlobalIdentificationList = globalIdentificationList,
                Name = name,
                LegalRegistration = legalRegistration,
                Contact = contact,
                PostalAddress = postalAddress as Models.BasicWL.PostalAddress,
                ElectronicAddress = electronicAddress,
                VatRegistrationList = vatRegistrationList,
            };
        }

        internal SalesAgent? GetSalesAgent()
        {
            if (_salesAgentNode == null)
                return null;

            var name = GetName(_salesAgentNode);
            var id = GetIdList(_salesAgentNode)?.FirstOrDefault();
            var globalIdentificationList = GetGlobalIdentificationList(_salesAgentNode);
            var legalRegistration =
                GetLegalRegistration(_salesAgentNode) as Models.Extended.LegalRegistration;
            var contact = GetContact(_salesAgentNode) as Contact;

            var postalAddress = GetPostalAddress(_salesAgentNode) as Models.BasicWL.PostalAddress;

            var electronicAddress = GetElectronicAddress(_salesAgentNode);
            var vatRegistration = GetVatRegistration(_salesAgentNode);

            return new SalesAgent
            {
                Id = id,
                GlobalIdentificationList = globalIdentificationList,
                Name = name,
                LegalRegistration = legalRegistration,
                Contact = contact,
                PostalAddress = postalAddress,
                ElectronicAddress = electronicAddress,
                VatRegistration = vatRegistration,
            };
        }

        internal Models.Minimum.Seller? GetSeller()
        {
            var postalAddress = GetPostalAddress(_sellerNode);

            var name = GetName(_sellerNode);

            var legalRegistration = GetLegalRegistration(_sellerNode);

            var vatRegistration = GetVatRegistration(_sellerNode);

            if (ConformanceLevelType == FacturXConformanceLevelType.Minimum)
            {
                return new Models.Minimum.Seller
                {
                    Name = name,
                    VatRegistration = vatRegistration,
                    LegalRegistration = legalRegistration,
                    PostalAddress = postalAddress,
                };
            }

            var idList = GetIdList(_sellerNode);
            var globalIdentificationList = GetGlobalIdentificationList(_sellerNode);
            var electronicAddress = GetElectronicAddress(_sellerNode);

            if (
                ConformanceLevelType == FacturXConformanceLevelType.BasicWL
                || ConformanceLevelType == FacturXConformanceLevelType.Basic
            )
            {
                return new Models.BasicWL.Seller
                {
                    Name = name,
                    VatRegistration = vatRegistration,
                    LegalRegistration = legalRegistration as Models.BasicWL.LegalRegistration,
                    ElectronicAddress = electronicAddress,
                    PostalAddress = postalAddress as Models.BasicWL.PostalAddress,
                    GlobalIdentificationList = globalIdentificationList,
                    IdList = idList,
                };
            }

            var contact = GetContact(_sellerNode);
            var additionalLegalInformation = _sellerNode
                .SelectSingleNode("*[local-name() = 'Description']")
                ?.InnerText;

            var fiscalRegistration = GetFiscalRegistration(_sellerNode);

            if (ConformanceLevelType == FacturXConformanceLevelType.EN16931)
            {
                return new Models.EN16931.Seller
                {
                    Name = name,
                    VatRegistration = vatRegistration,
                    IdList = idList,
                    GlobalIdentificationList = globalIdentificationList,
                    ElectronicAddress = electronicAddress,
                    AdditionalLegalInformation = additionalLegalInformation,
                    FiscalRegistration = fiscalRegistration,
                    PostalAddress = postalAddress as Models.BasicWL.PostalAddress,
                    LegalRegistration = legalRegistration as Models.BasicWL.LegalRegistration,
                    Contact = contact,
                };
            }

            var extendedSeller = new Models.Extended.Seller
            {
                Name = name,
                VatRegistration = vatRegistration,
                IdList = idList,
                GlobalIdentificationList = globalIdentificationList,
                ElectronicAddress = electronicAddress,
                AdditionalLegalInformation = additionalLegalInformation,
                FiscalRegistration = fiscalRegistration,
                Contact = contact as Models.Extended.Contact,
                LegalRegistration = legalRegistration as Models.Extended.LegalRegistration,
                PostalAddress = postalAddress as Models.BasicWL.PostalAddress,
            };

            return extendedSeller;
        }

        internal Models.BasicWL.SellerTaxRepresentative? GetSellerTaxRepresentative()
        {
            if (_sellerTaxRepresentativeNode == null)
                return null;

            var name = GetName(_sellerTaxRepresentativeNode);
            var postalAddress = GetPostalAddress(_sellerTaxRepresentativeNode);

            var vatRegistration = GetVatRegistration(_sellerTaxRepresentativeNode);

            if (
                ConformanceLevelType == FacturXConformanceLevelType.BasicWL
                || ConformanceLevelType == FacturXConformanceLevelType.Basic
                || ConformanceLevelType == FacturXConformanceLevelType.EN16931
            )
            {
                return new SellerTaxRepresentative
                {
                    Name = name,
                    PostalAddress = postalAddress as Models.BasicWL.PostalAddress,
                    VatRegistration = vatRegistration,
                };
            }

            var id = GetIdList(_sellerTaxRepresentativeNode)?.FirstOrDefault();
            var globalIdentificationList = GetGlobalIdentificationList(
                _sellerTaxRepresentativeNode
            );
            var legalRegistration = GetLegalRegistration(_sellerTaxRepresentativeNode);
            var contact = GetContact(_sellerTaxRepresentativeNode);
            var electronicAddress = GetElectronicAddress(_sellerTaxRepresentativeNode);

            return new Models.Extended.SellerTaxRepresentative
            {
                Name = name,
                PostalAddress = postalAddress as Models.BasicWL.PostalAddress,
                VatRegistration = vatRegistration,
                Id = id,
                GlobalIdentificationList = globalIdentificationList,
                LegalRegistration = legalRegistration as Models.Extended.LegalRegistration,
                Contact = contact as Models.Extended.Contact,
                ElectronicAddress = electronicAddress,
            };
        }

        internal LineDeliverAddress? GetLineDeliverToAddress(XmlNode lineNode)
        {
            var deliverToAddressNode = lineNode
                .SelectSingleNode("*[local-name() = 'SpecifiedLineTradeDelivery']")
                ?.SelectSingleNode("*[local-name() = 'ShipToTradeParty']");

            if (deliverToAddressNode == null)
                return null;

            var id = GetIdList(deliverToAddressNode)?.FirstOrDefault();
            var globalIdentificationList = GetGlobalIdentificationList(deliverToAddressNode);
            var name = GetName(deliverToAddressNode);
            var legalRegistration =
                GetLegalRegistration(deliverToAddressNode) as Models.Extended.LegalRegistration;
            var contact = GetContact(deliverToAddressNode) as Contact;

            var postalAddress =
                GetPostalAddress(deliverToAddressNode) as Models.BasicWL.PostalAddress;

            var electronicAddress = GetElectronicAddress(deliverToAddressNode);
            var vatRegistration = GetVatRegistration(deliverToAddressNode);

            return new LineDeliverAddress
            {
                Id = id,
                GlobalIdentificationList = globalIdentificationList,
                Name = name,
                LegalRegistration = legalRegistration,
                ElectronicAddress = electronicAddress,
                VatRegistration = vatRegistration,
                Contact = contact,
                PostalAddress = postalAddress,
            };
        }

        internal IActor? GetLineUltimateDeliverToAddress(XmlNode lineNode)
        {
            var ultimateDeliverToAddressNode = lineNode
                .SelectSingleNode("*[local-name() = 'SpecifiedLineTradeDelivery']")
                ?.SelectSingleNode("*[local-name() = 'UltimateShipToTradeParty']");

            if (ultimateDeliverToAddressNode == null)
                return null;

            var name = GetName(ultimateDeliverToAddressNode);
            var id = GetIdList(ultimateDeliverToAddressNode)?.FirstOrDefault();
            var globalIdentificationList = GetGlobalIdentificationList(
                ultimateDeliverToAddressNode
            );
            var legalRegistration =
                GetLegalRegistration(ultimateDeliverToAddressNode)
                as Models.Extended.LegalRegistration;
            var contact = GetContact(ultimateDeliverToAddressNode) as Contact;

            var postalAddress =
                GetPostalAddress(ultimateDeliverToAddressNode) as Models.BasicWL.PostalAddress;

            var electronicAddress = GetElectronicAddress(ultimateDeliverToAddressNode);
            var vatRegistration = GetVatRegistration(ultimateDeliverToAddressNode);

            return new Models.Extended.LineDeliverAddress
            {
                Id = id,
                GlobalIdentificationList = globalIdentificationList,
                Name = name,
                LegalRegistration = legalRegistration,
                Contact = contact,
                PostalAddress = postalAddress,
                ElectronicAddress = electronicAddress,
                VatRegistration = vatRegistration,
            };
        }
    }
}
