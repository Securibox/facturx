using System.Xml;
using Securibox.FacturX.Core.References;
using Securibox.FacturX.Models;
using Securibox.FacturX.Models.Enums;

namespace Securibox.FacturX.Core
{
    internal class ReferenceFactory
    {
        private readonly ReferenceConverter _referenceConverter;
        private readonly FacturXConformanceLevelType _conformanceLevelType;
        private readonly XmlDocument _xmlDocument;

        public ReferenceFactory(
            FacturXConformanceLevelType conformanceLevelType,
            XmlDocument xmlDocument
        )
        {
            _referenceConverter = new ReferenceConverter(conformanceLevelType, xmlDocument);
            _conformanceLevelType = conformanceLevelType;
            _xmlDocument = xmlDocument;
        }

        internal IReference? ConvertInvoiceReference(ReferenceType type)
        {
            switch (type)
            {
                case ReferenceType.BuyerReference:
                    return _referenceConverter.GetBuyerReference();

                case ReferenceType.BuyerAccountingReference:
                    return _referenceConverter.GetBuyerAccountingReference();

                case ReferenceType.ContractReference:
                    return _referenceConverter.GetContractReference();

                case ReferenceType.DeliveryNoteReference:
                    return _referenceConverter.GetDeliveryNoteReference();

                case ReferenceType.DespatchAdviceReference:
                    return _referenceConverter.GetDespacthAdviceReference();

                case ReferenceType.InvoicedObjectIdentifier:
                    return _referenceConverter.GetInvoicedObjectIdentifier();

                case ReferenceType.ProjectReference:
                    return _referenceConverter.GetProjectReference();

                case ReferenceType.PurchaseOrderReference:
                    return _referenceConverter.GetPurchaseOrderReference();

                case ReferenceType.QuotationReference:
                    return _referenceConverter.GetQuotationReference();

                case ReferenceType.ReceivingAdviceReference:
                    return _referenceConverter.GetReceivingAdviceReference();

                case ReferenceType.TenderOrLotReference:
                    return _referenceConverter.GetTenderOrLotReference();

                case ReferenceType.SalesOrderReference:
                    return _referenceConverter.GetSalesOrderReference();

                default:
                    return null;
            }
        }

        internal IEnumerable<IReference>? ConvertInvoiceReferenceList(ReferenceType type)
        {
            switch (type)
            {
                case ReferenceType.PreviousInvoiceReferenceList:
                    return _referenceConverter.GetPreviousInvoiceReferenceList();

                case ReferenceType.SupportingDocumentReference:
                    return _referenceConverter.GetSupportingDocumentReferenceList();

                case ReferenceType.UltimateCustomerOrderReferenceList:
                    return _referenceConverter.GetUltimateCustomerOrderReferenceList();

                default:
                    return null;
            }
        }

        internal IReference? ConvertLineReference(XmlNode lineNode, ReferenceType type)
        {
            switch (type)
            {
                case ReferenceType.BuyerAccountingReference:
                    return _referenceConverter.GetLineBuyerAccountingReference(lineNode);

                case ReferenceType.ContractReference:
                    return _referenceConverter.GetLineContractReference(lineNode);

                case ReferenceType.DeliveryNoteReference:
                    return _referenceConverter.GetLineDeliveryNoteReference(lineNode);

                case ReferenceType.DespatchAdviceReference:
                    return _referenceConverter.GetLineDespacthAdviceReference(lineNode);

                case ReferenceType.InvoicedObjectIdentifier:
                    return _referenceConverter.GetLineInvoicedObjectIdentifier(lineNode);

                case ReferenceType.PurchaseOrderReference:
                    return _referenceConverter.GetLinePurchaseOrderReference(lineNode);

                case ReferenceType.QuotationReference:
                    return _referenceConverter.GetLineQuotationReference(lineNode);

                case ReferenceType.ReceivingAdviceReference:
                    return _referenceConverter.GetLineReceivingAdviceReference(lineNode);

                case ReferenceType.PreviousInvoiceReference:
                    return _referenceConverter.GetLinePreviousInvoiceReference(lineNode);

                default:
                    return null;
            }
        }

        internal IEnumerable<IReference>? ConvertLineReferenceList(
            XmlNode lineNode,
            ReferenceType type
        )
        {
            switch (type)
            {
                case ReferenceType.LineAdditionalDocumentReferenceList:
                    return _referenceConverter.GetLineAdditionalDocumentReferenceList(lineNode);

                case ReferenceType.UltimateCustomerOrderReferenceList:
                    return _referenceConverter.GetLineUltimateCustomerOrderReferenceList(lineNode);

                default:
                    return null;
            }
        }
    }
}
